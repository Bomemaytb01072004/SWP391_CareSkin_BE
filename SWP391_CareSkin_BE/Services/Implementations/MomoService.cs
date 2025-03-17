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

        public MomoService(IMomoRepository momoRepository, IOrderRepository orderRepository, IMapper mapper, ILogger<MomoService> logger)
        {
            _momoRepository = momoRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
            _logger = logger;
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

        public MomoPaymentResponseDto CreateErrorResponse(string message, long amount)
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

                // Extract orderId from Momo's orderId (remove any prefix and timestamp)
                var orderIdStr = callbackDto.OrderId;
                
                // Handle different orderId formats
                if (orderIdStr.StartsWith("ORDER_"))
                {
                    orderIdStr = orderIdStr.Replace("ORDER_", "");
                }
                
                // Handle Partner_Transaction_ID format (from IPN)
                if (orderIdStr.StartsWith("Partner_Transaction_ID_"))
                {
                    orderIdStr = orderIdStr.Replace("Partner_Transaction_ID_", "");
                }

                // If there's a timestamp or other suffix (format: 123_1712187247), get the part before the underscore
                if (orderIdStr.Contains("_"))
                {
                    orderIdStr = orderIdStr.Split('_')[0];
                }

                if (!int.TryParse(orderIdStr, out int orderId))
                {
                    _logger.LogWarning("Invalid order ID format in Momo callback: {OrderId}", callbackDto.OrderId);
                    throw new InvalidOperationException($"Invalid order ID format: {callbackDto.OrderId}");
                }

                // Update payment status if payment is successful (resultCode = 0)
                if (callbackDto.ResultCode == 0)
                {
                    _logger.LogInformation("Successful payment for order {OrderId}, TransId={TransId}", orderId, callbackDto.TransId);
                    
                    // Find payment by order ID
                    var payment = await _momoRepository.GetMomoPaymentByOrderIdAsync(orderId);
                    if (payment != null)
                    {
                        // Verify amount matches to prevent fraud
                        if (payment.Amount != callbackDto.Amount)
                        {
                            _logger.LogWarning("Amount mismatch for order {OrderId}: Expected {Expected}, Got {Actual}",
                                orderId, payment.Amount, callbackDto.Amount);
                            return;
                        }

                        // Update payment status to paid
                        payment.IsPaid = true;
                        payment.PaymentTime = DateTime.UtcNow;
                        payment.TransactionId = callbackDto.TransId.ToString();
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
                    else
                    {
                        _logger.LogWarning("No payment record found for order {OrderId}", orderId);
                    }
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
                throw; // Rethrow to let the controller handle it
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
