using AutoMapper;
using SWP391_CareSkin_BE.DTOS.Requests.Momo;
using SWP391_CareSkin_BE.DTOS.Responses.Momo;
using SWP391_CareSkin_BE.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Services.Implementations
{
    public class MomoService : IMomoService
    {
        private readonly IMomoRepository _momoRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<MomoService> _logger;
        private readonly IEmailService _emailService;

        public MomoService(
            IMomoRepository momoRepository, 
            IOrderRepository orderRepository, 
            IMapper mapper, 
            ILogger<MomoService> logger,
            IEmailService emailService)
        {
            _momoRepository = momoRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
            _logger = logger;
            _emailService = emailService;
        }

        public async Task<MomoPaymentResponseDto> CreateMomoPaymentAsync(MomoPaymentRequestDto paymentRequestDto)
        {
            try
            {
                // Check if order exists
                var order = await _orderRepository.GetOrderByIdAsync(paymentRequestDto.OrderId);
                if (order == null)
                {
                    return CreateErrorResponse("Bad format request.", paymentRequestDto.Amount);
                }

                // Kiểm tra nếu đơn hàng đã thanh toán
                var existingPayment = await _momoRepository.GetMomoPaymentByOrderIdAsync(paymentRequestDto.OrderId);
                if (existingPayment != null && existingPayment.IsPaid)
                {
                    return CreateErrorResponse("Bad format request.", paymentRequestDto.Amount);
                }

                // Create MomoPayment entity
                var momoPayment = new MomoPayment
                {
                    MomoPaymentId = Guid.NewGuid().ToString(),
                    OrderId = paymentRequestDto.OrderId,
                    Amount = paymentRequestDto.Amount,
                    OrderInfo = $"Thanh toán đơn hàng #{paymentRequestDto.OrderId} - CareSkin",
                    CreatedDate = DateTime.UtcNow,
                    IsPaid = false,
                    IsExpired = false
                };

                // Save payment to database
                await _momoRepository.SaveMomoPaymentAsync(momoPayment);

                // Create Momo payment request
                var response = await _momoRepository.CreatePaymentAsync(momoPayment);

                // If request failed, log the error
                if (response.ErrorCode != 0)
                {
                    _logger.LogError("Momo payment request failed for order {OrderId}: {ErrorMessage}",
                        paymentRequestDto.OrderId, response.Message);
                    
                    // Override with standardized error response
                    return CreateErrorResponse("Bad format request.", paymentRequestDto.Amount);
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Momo payment for order {OrderId}", paymentRequestDto.OrderId);
                return CreateErrorResponse("Bad format request.", paymentRequestDto.Amount);
            }
        }

        public MomoPaymentResponseDto CreateErrorResponse(string message, decimal amount)
        {
            return new MomoPaymentResponseDto
            {
                PayUrl = "",
                Deeplink = "",
                QrCodeUrl = "",
                ErrorCode = -1,  // Using -1 as the error code for the service layer
                Message = message,
                RequestId = "",
                Amount = amount,
                ResponseTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };
        }

        public async Task HandleMomoCallbackAsync(MomoCallbackDto callbackDto)
        {
            try
            {
                _logger.LogInformation("Processing Momo callback: OrderId={OrderId}, ResultCode={ResultCode}, TransId={TransId}",
                    callbackDto.OrderId, callbackDto.ResultCode, callbackDto.TransId);

                // Create callback record with current timestamp
                var callback = _mapper.Map<MomoCallback>(callbackDto);
                callback.ReceivedDate = DateTime.UtcNow;
                await _momoRepository.SaveMomoCallbackAsync(callback);

                // Extract orderId from Momo's orderId format
                if (!TryExtractOrderId(callbackDto.OrderId, out int orderId))
                {
                    _logger.LogWarning("Invalid order ID format in Momo callback: {OrderId}. Attempting to find payment by Momo order ID.", callbackDto.OrderId);
                    
                    // Try to get payment by Momo order ID directly
                    var payment = await _momoRepository.GetMomoPaymentByMomoOrderIdAsync(callbackDto.OrderId);
                    if (payment != null)
                    {
                        orderId = payment.OrderId;
                        _logger.LogInformation("Found payment for Momo order ID: {MomoOrderId}, mapped to order ID: {OrderId}", callbackDto.OrderId, orderId);
                    }
                    else
                    {
                        _logger.LogError("Could not find payment for Momo order ID: {MomoOrderId}", callbackDto.OrderId);
                        return;
                    }
                }

                // Update payment status based on result code
                if (callbackDto.ResultCode == 0)
                {
                    await HandleSuccessfulPayment(orderId, callbackDto);
                }
                else
                {
                    _logger.LogWarning("Failed payment for order {OrderId}, ResultCode={ResultCode}, Message={Message}",
                        orderId, callbackDto.ResultCode, callbackDto.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling Momo callback for order {OrderId}", callbackDto.OrderId);
                throw;
            }
        }

        /// <summary>
        /// Extracts the order ID from various Momo callback formats
        /// </summary>
        private bool TryExtractOrderId(string orderIdStr, out int orderId)
        {
            orderId = 0;
            
            if (string.IsNullOrEmpty(orderIdStr))
            {
                _logger.LogWarning("Empty order ID in Momo callback");
                return false;
            }
            
            _logger.LogInformation("Attempting to extract order ID from: {OrderIdStr}", orderIdStr);
                
            // Remove common prefixes
            string processedOrderId = orderIdStr;
            
            if (processedOrderId.StartsWith("ORDER_"))
                processedOrderId = processedOrderId.Replace("ORDER_", "");
            else if (processedOrderId.StartsWith("Partner_Transaction_ID_"))
                processedOrderId = processedOrderId.Replace("Partner_Transaction_ID_", "");

            // If there's a timestamp or other suffix (format: 123_1712187247), get the part before the underscore
            if (processedOrderId.Contains("_"))
                processedOrderId = processedOrderId.Split('_')[0];

            bool success = int.TryParse(processedOrderId, out orderId);
            
            if (success)
            {
                _logger.LogInformation("Successfully extracted order ID: {OrderId} from: {OrderIdStr}", orderId, orderIdStr);
            }
            else
            {
                _logger.LogWarning("Failed to parse order ID from: {ProcessedOrderId} (original: {OrderIdStr})", processedOrderId, orderIdStr);
            }
            
            return success;
        }

        /// <summary>
        /// Handles a successful payment (ResultCode = 0)
        /// </summary>
        private async Task HandleSuccessfulPayment(int orderId, MomoCallbackDto callbackDto)
        {
            _logger.LogInformation("Successful payment for order {OrderId}, TransId={TransId}", orderId, callbackDto.TransId);
            
            // Find payment by order ID
            var payment = await _momoRepository.GetMomoPaymentByOrderIdAsync(orderId);
            if (payment == null)
            {
                _logger.LogWarning("No payment record found for order {OrderId}", orderId);
                return;
            }

            // Verify amount matches to prevent fraud
            if (payment.Amount != callbackDto.Amount)
            {
                _logger.LogWarning("Amount mismatch for order {OrderId}: Expected {Expected}, Got {Actual}",
                    orderId, payment.Amount, callbackDto.Amount);
                return;
            }

            // Update payment status to paid
            payment.PaymentMethod = "MoMo";
            payment.Status = callbackDto.ResultCode == 0 ? "Successful" : "Failed";
            payment.ResponseMessage = callbackDto.Message;
            payment.ResponseCode = callbackDto.ResultCode.ToString();
            payment.TransactionId = callbackDto.TransId;
            payment.PaymentTime = DateTime.Now;
            payment.IsPaid = callbackDto.ResultCode == 0;
            await _momoRepository.UpdatePaymentAsync(payment);

            // Update order status to Paid (assuming 3 is the Paid status ID)
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order != null)
            {
                order.OrderStatusId = 3; // 3 is Paid status
                await _orderRepository.UpdateOrderAsync(order);
                _logger.LogInformation("Order {OrderId} status updated to Paid", orderId);
            }
            else
            {
                _logger.LogWarning("Order {OrderId} not found when updating payment status", orderId);
            }
        }

        public bool ValidateMomoCallback(MomoCallbackDto callbackDto)
        {
            try
            {
                // Verify signature to ensure the callback is legitimate
                bool isValid = _momoRepository.VerifySignature(callbackDto);
                
                if (!isValid)
                {
                    _logger.LogWarning("Invalid signature in Momo callback: OrderId={OrderId}, TransId={TransId}",
                        callbackDto.OrderId, callbackDto.TransId);
                }
                
                return isValid;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating Momo callback signature");
                return false;
            }
        }

        public async Task<MomoPaymentStatusDto> CheckPaymentStatusAsync(int orderId)
        {
            var payment = await _momoRepository.GetMomoPaymentByOrderIdAsync(orderId);
            if (payment == null)
            {
                throw new InvalidOperationException($"No payment found for order {orderId}");
            }

            // Get the latest callback for this order
            var callbacks = await _momoRepository.GetCallbacksForOrderAsync(orderId);
            var latestCallback = callbacks.OrderByDescending(c => c.ReceivedDate).FirstOrDefault();

            return new MomoPaymentStatusDto
            {
                OrderId = orderId,
                Amount = payment.Amount,
                IsPaid = payment.IsPaid,
                PaymentTime = payment.PaymentTime,
                TransactionId = payment.TransactionId,
                ResultCode = latestCallback?.ResultCode ?? -1,
                Message = latestCallback?.Message ?? "No callback received"
            };
        }

        public async Task CancelExpiredPayments()
        {
            try
            {
                var expiredPayments = await _momoRepository.GetExpiredPaymentsAsync();
                foreach (var payment in expiredPayments)
                {
                    payment.IsExpired = true;
                    await _momoRepository.UpdatePaymentAsync(payment);
                    _logger.LogInformation("Marked payment {PaymentId} for order {OrderId} as expired",
                        payment.MomoPaymentId, payment.OrderId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling expired payments");
            }
        }
    }
}
