using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.DTOS.Requests.Momo;
using SWP391_CareSkin_BE.DTOS.Responses.Momo;
using SWP391_CareSkin_BE.Models;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using SWP391_CareSkin_BE.Repositories.Interfaces;

namespace SWP391_CareSkin_BE.Repositories
{
    public class MomoRepository : IMomoRepository
    {
        private readonly MomoConfig _momoConfig;
        private readonly MyDbContext _dbContext;
        private readonly ILogger<MomoRepository> _logger;

        public MomoRepository(IOptions<MomoConfig> momoConfig, MyDbContext dbContext, ILogger<MomoRepository> logger)
        {
            _momoConfig = momoConfig.Value;
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<MomoPaymentResponseDto> CreatePaymentAsync(MomoPayment payment)
        {
            var existingPayment = await _dbContext.MomoPayments
                .Where(p => p.OrderId == payment.OrderId && !p.IsPaid && !p.IsExpired)
                .FirstOrDefaultAsync();

            if (existingPayment != null)
            {
                // Nếu giao dịch chưa thanh toán, cập nhật OrderId mới
                payment.OrderInfo = $"Thanh toán đơn hàng #{payment.OrderId} - CareSkin (Lần {DateTimeOffset.UtcNow.ToUnixTimeSeconds()})";
            }

            try
            {
                // Generate a unique requestId
                string requestId = Guid.NewGuid().ToString();
                
                // Format orderId as string with ORDER_ prefix and timestamp to ensure uniqueness
                string orderId = $"ORDER_{payment.OrderId}_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";

                // Amount is already in VND, no need to multiply
                long amountInVnd = payment.Amount;

                // Create raw data for signature
                string rawData = $"accessKey={_momoConfig.AccessKey}"
                               + $"&amount={amountInVnd}"
                               + $"&extraData="
                               + $"&ipnUrl={_momoConfig.IpnUrl}"
                               + $"&orderId={orderId}"
                               + $"&orderInfo={payment.OrderInfo}"
                               + $"&partnerCode={_momoConfig.PartnerCode}"
                               + $"&redirectUrl={_momoConfig.ReturnUrl}"
                               + $"&requestId={requestId}"
                               + $"&requestType={_momoConfig.RequestType}";

                string signature = SignSHA256(rawData, _momoConfig.SecretKey);

                // Create request object for Momo
                var requestBody = new
                {
                    partnerCode = _momoConfig.PartnerCode,
                    partnerName = "CareSkin",
                    storeId = "CareSkinStore",
                    requestId,
                    amount = amountInVnd,
                    orderId,
                    orderInfo = payment.OrderInfo,
                    redirectUrl = _momoConfig.ReturnUrl,
                    ipnUrl = _momoConfig.IpnUrl,
                    lang = "vi",
                    extraData = "",
                    requestType = _momoConfig.RequestType,
                    signature
                };

                // Send POST request to Momo
                using var client = new HttpClient();
                var json = JsonSerializer.Serialize(requestBody);
                
                // Log the request JSON
                _logger.LogInformation("Sending request to Momo: {Json}", json);
                
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(_momoConfig.MomoApiUrl, httpContent);
                var responseContent = await response.Content.ReadAsStringAsync();
                
                // Log response
                _logger.LogInformation("Momo Payment Response: {ResponseContent}", responseContent);
                
                if (response.IsSuccessStatusCode)
                {
                    var responseData = JsonSerializer.Deserialize<MomoPaymentResponseDto>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    
                    // Đảm bảo các trường không bị null
                    responseData.ResponseTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                    responseData.Amount = payment.Amount;
                    responseData.OrderId = orderId;
                    
                    return responseData;
                }
                else
                {
                    return new MomoPaymentResponseDto
                    {
                        PayUrl = "",
                        Deeplink = "",
                        QrCodeUrl = "",
                        ErrorCode = -1,
                        Message = "Bad format request.",
                        RequestId = "",
                        Amount = payment.Amount,
                        ResponseTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Momo payment");
                return new MomoPaymentResponseDto
                {
                    PayUrl = "",
                    Deeplink = "",
                    QrCodeUrl = "",
                    ErrorCode = -1,
                    Message = "Bad format request.",
                    RequestId = "",
                    Amount = payment.Amount,
                    ResponseTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                };
            }
        }

        public bool VerifySignature(MomoCallbackDto callbackDto)
        {
            // According to Momo documentation, raw data for callback signature verification
            string rawData = $"accessKey={_momoConfig.AccessKey}"
                           + $"&amount={callbackDto.Amount}"
                           + $"&extraData={callbackDto.ExtraData}"
                           + $"&message={callbackDto.Message}"
                           + $"&orderId={callbackDto.OrderId}"
                           + $"&orderInfo=Thanh toán đơn hàng #{callbackDto.OrderId} - CareSkin"
                           + $"&orderType={_momoConfig.RequestType}"
                           + $"&partnerCode={_momoConfig.PartnerCode}"
                           + $"&payType={callbackDto.PayType}"
                           + $"&requestId={callbackDto.RequestId}"
                           + $"&responseTime=&resultCode={callbackDto.ResultCode}"
                           + $"&transId={callbackDto.TransId}";

            string calculatedSignature = SignSHA256(rawData, _momoConfig.SecretKey);
            return callbackDto.Signature == calculatedSignature;
        }

        public async Task SaveMomoPaymentAsync(MomoPayment payment)
        {
            await _dbContext.MomoPayments.AddAsync(payment);
            await _dbContext.SaveChangesAsync();
        }

        public async Task SaveMomoCallbackAsync(MomoCallback callback)
        {
            await _dbContext.MomoCallbacks.AddAsync(callback);
            await _dbContext.SaveChangesAsync();
        }
        
        public async Task<MomoPayment?> GetMomoPaymentByOrderIdAsync(int orderId)
        {
            return await _dbContext.MomoPayments
                .Where(p => p.OrderId == orderId && !p.IsExpired)
                .OrderByDescending(p => p.CreatedDate)
                .FirstOrDefaultAsync();
        }
        
        public async Task UpdateMomoPaymentStatusAsync(string momoPaymentId, bool isPaid)
        {
            var payment = await _dbContext.MomoPayments
                .FirstOrDefaultAsync(p => p.MomoPaymentId == momoPaymentId);
                
            if (payment != null)
            {
                payment.IsPaid = isPaid;
                payment.PaymentTime = isPaid ? DateTime.UtcNow : null;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<MomoCallback?> GetLatestCallbackForOrderAsync(string orderId)
        {
            return await _dbContext.MomoCallbacks
                .Where(c => c.OrderId == orderId)
                .OrderByDescending(c => c.MomoCallbackId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<MomoPayment>> GetExpiredPaymentsAsync()
        {
            // Lấy các payment chưa thanh toán và tạo cách đây hơn 15 phút
            return await _dbContext.MomoPayments
                .Where(p => !p.IsPaid && !p.IsExpired && p.CreatedDate < DateTime.UtcNow.AddMinutes(-15))
                .ToListAsync();
        }

        public async Task UpdatePaymentAsync(MomoPayment payment)
        {
            _dbContext.MomoPayments.Update(payment);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<MomoPayment?> GetMomoPaymentByMomoOrderIdAsync(string momoOrderId)
        {
            // Lấy orderId từ momoOrderId (bỏ "ORDER_" và lấy phần số đầu tiên)
            var orderIdStr = momoOrderId.Replace("ORDER_", "");
            
            // Nếu có timestamp (format: ORDER_123_1712187247), lấy phần trước dấu _
            int orderId;
            if (orderIdStr.Contains("_"))
            {
                var parts = orderIdStr.Split('_');
                if (!int.TryParse(parts[0], out orderId))
                {
                    return null;
                }
            }
            else if (!int.TryParse(orderIdStr, out orderId))
            {
                return null;
            }

            // Tìm payment dựa trên orderId
            return await _dbContext.MomoPayments
                .Where(p => p.OrderId == orderId && !p.IsExpired)
                .OrderByDescending(p => p.CreatedDate)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<MomoCallback>> GetCallbacksForOrderAsync(int orderId)
        {
            return await _dbContext.MomoCallbacks
                .Where(c => c.OrderId == orderId.ToString())
                .OrderByDescending(c => c.MomoCallbackId)
                .ToListAsync();
        }

        private string SignSHA256(string data, string secretKey)
        {
            using var hmacSha256 = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey));
            var hash = hmacSha256.ComputeHash(Encoding.UTF8.GetBytes(data));
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}
