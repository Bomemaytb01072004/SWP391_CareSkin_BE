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
            // Create callback record
            var callback = _mapper.Map<MomoCallback>(callbackDto);
            await _momoRepository.SaveMomoCallbackAsync(callback);

            // Extract orderId from Momo's orderId (remove "ORDER_" prefix and any timestamp)
            var orderIdStr = callbackDto.OrderId.Replace("ORDER_", "");

            // Nếu có timestamp (format: ORDER_123_1712187247), lấy phần trước dấu _
            if (orderIdStr.Contains("_"))
            {
                orderIdStr = orderIdStr.Split('_')[0];
            }

            if (!int.TryParse(orderIdStr, out int orderId))
            {
                throw new InvalidOperationException($"Invalid order ID format: {callbackDto.OrderId}");
            }

            // Update payment status if payment is successful
            if (callbackDto.ResultCode == 0) // 0 means success in Momo
            {
                // Tìm payment dựa trên Momo's orderId (giữ nguyên format từ Momo)
                var payment = await _momoRepository.GetMomoPaymentByMomoOrderIdAsync(callbackDto.OrderId);
                if (payment != null)
                {
                    await _momoRepository.UpdateMomoPaymentStatusAsync(payment.MomoPaymentId, true);

                    // Update order status to Paid
                    var order = await _orderRepository.GetOrderByIdAsync(orderId);
                    if (order != null)
                    {
                        order.OrderStatusId = 3; // 3 is Paid status
                        await _orderRepository.UpdateOrderAsync(order);
                    }
                }
            }
        }

        public bool ValidateMomoCallback(MomoCallbackDto callbackDto)
        {
            return _momoRepository.VerifySignature(callbackDto);
        }

        public async Task<MomoPaymentStatusDto> CheckPaymentStatusAsync(int orderId)
        {
            var payment = await _momoRepository.GetMomoPaymentByOrderIdAsync(orderId);
            if (payment == null)
            {
                throw new InvalidOperationException($"No payment found for order {orderId}");
            }

            var callback = await _momoRepository.GetLatestCallbackForOrderAsync($"ORDER_{orderId}");

            return new MomoPaymentStatusDto
            {
                OrderId = orderId,
                Amount = payment.Amount,
                IsPaid = payment.IsPaid,
                PaymentTime = payment.PaymentTime
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
