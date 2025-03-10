using SWP391_CareSkin_BE.DTOs.Requests.Vnpay;
using SWP391_CareSkin_BE.DTOs.Responses.Vnpay;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Mappers
{
    public static class VnpayMapper
    {
        public static VnpayTransactions ToEntity(VnpayRequestDTO model)
        {
            return new VnpayTransactions
            {
                
                OrderId = model.OrderId,
                Amount = model.Amount,
                OrderDescription = model.OrderDescription,
                PaymentMethod = "VnPay",
                PaymentStatus = "Pending",
                CreatedAt = DateTime.UtcNow
            };
        }

        // Từ Entity sang PaymentResponseModel (nếu muốn)
        public static VnpayResponseDTO ToResponse(this VnpayTransactions entity)
        {
            return new VnpayResponseDTO
            {
                OrderDescription = entity.OrderDescription,
                TransactionId = entity.TransactionId.ToString(),
                OrderId = entity.OrderId,
                PaymentMethod = entity.PaymentMethod,
                // ... tuỳ biến
                Success = entity.PaymentStatus == "Success"
            };
        }
    }
}
