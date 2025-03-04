using SWP391_CareSkin_BE.DTOs.Requests.Vnpay;
using SWP391_CareSkin_BE.DTOs.Responses.Vnpay;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Mappers
{
    public static class VnpayMapper
    {
        public static VnpayTransactions ToEntity(VnpayRequestDTO request)
        {
            return new VnpayTransactions
            {
                OrderId = request.OrderId,
                Amount = request.Amount,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };
        }

        public static VnpayResponseDTO ToResponse(string paymentUrl, VnpayRequestDTO request)
        {
            return new VnpayResponseDTO
            {
                PaymentUrl = paymentUrl,
                OrderId = request.OrderId,
                Amount = request.Amount,
                Status = "Pending"
            };
        }
    }
}
