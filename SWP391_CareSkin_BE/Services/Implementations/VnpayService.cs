using SWP391_CareSkin_BE.DTOs.Requests.Vnpay;
using SWP391_CareSkin_BE.DTOs.Responses.Vnpay;
using SWP391_CareSkin_BE.Mappers;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using SWP391_CareSkin_BE.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace SWP391_CareSkin_BE.Services.Implementations
{
    public class VnpayService : IVnpayService
    {
        private readonly string _vnpTmnCode;
        private readonly string _vnpHashSecret;
        private readonly string _vnpUrl;
        private readonly IVnpayRepository _vnpayRepository;

        public VnpayService(IConfiguration configuration, IVnpayRepository vnpayRepository)
        {
            _vnpTmnCode = configuration["Vnpay:TmnCode"];
            _vnpHashSecret = configuration["Vnpay:HashSecret"];
            _vnpUrl = configuration["Vnpay:Url"];
            _vnpayRepository = vnpayRepository;
        }

        public async Task<VnpayResponseDTO> CreatePaymentAsync(VnpayRequestDTO request)
        {
            var transaction = VnpayMapper.ToEntity(request);
            await _vnpayRepository.AddTransactionAsync(transaction);

            var timeStamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var vnpayData = new SortedDictionary<string, string>
            {
                { "vnp_Version", "2.1.0" },
                { "vnp_Command", "pay" },
                { "vnp_TmnCode", _vnpTmnCode },
                { "vnp_Amount", ((int)request.Amount * 100).ToString() },
                { "vnp_TxnRef", request.OrderId },
                { "vnp_CreateDate", timeStamp }
            };

            string queryString = string.Join("&", vnpayData.Select(kvp => $"{kvp.Key}={kvp.Value}"));
            string secureHash = ComputeHmacSHA512(queryString, _vnpHashSecret);
            string paymentUrl = $"{_vnpUrl}?{queryString}&vnp_SecureHash={secureHash}";

            return VnpayMapper.ToResponse(paymentUrl, request);
        }

        public async Task<bool> ProcessPaymentCallbackAsync(Dictionary<string, string> queryParams)
        {
            if (!ValidateSignature(queryParams))
            {
                return false; // Chữ ký không hợp lệ, có thể là request giả mạo
            }

            string orderId = queryParams["vnp_TxnRef"];
            string responseCode = queryParams["vnp_ResponseCode"];

            var transaction = await _vnpayRepository.GetTransactionByOrderIdAsync(orderId);
            if (transaction == null)
            {
                return false; // Không tìm thấy giao dịch
            }
            // Kiểm tra mã phản hồi từ VNPAY
            if (responseCode == "00")
            {
                transaction.Status = "Success";
            }
            else
            {
                transaction.Status = "Failed";
            }

            await _vnpayRepository.UpdateTransactionAsync(transaction);
            return true;
        }

        public bool ValidateSignature(Dictionary<string, string> queryParams)
        {
            if (!queryParams.ContainsKey("vnp_SecureHash")) return false;

            string receivedHash = queryParams["vnp_SecureHash"];
            queryParams.Remove("vnp_SecureHash");

            string generatedHash = ComputeHmacSHA512(string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={kvp.Value}")), _vnpHashSecret);
            return receivedHash.Equals(generatedHash, StringComparison.OrdinalIgnoreCase);
        }
        private string ComputeHmacSHA512(string data, string key)
        {
            using (var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key)))
            {
                return BitConverter.ToString(hmac.ComputeHash(Encoding.UTF8.GetBytes(data))).Replace("-", "").ToLower();
            }
        }


    }

}
