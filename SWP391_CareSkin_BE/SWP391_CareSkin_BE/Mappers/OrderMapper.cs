using SWP391_CareSkin_BE.DTOs.Requests;
using SWP391_CareSkin_BE.DTOs.Responses;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Mappers
{
    public class OrderMapper
    {
        // Chuyển từ Order Entity sang OrderDTO
        public static OrderDTO ToDTO(Order order)
        {
            if (order == null)
                return null;

            return new OrderDTO
            {
                OrderId = order.OrderId,
                CustomerId = order.CustomerId,
                OrderStatusId = order.OrderStatusId,
                OrderStatusName = order.OrderStatus?.OrderStatusName,
                PromotionId = order.PromotionId,
                PromotionName = order.Promotion?.Name,
                TotalPrice = order.TotalPrice,
                OrderDate = order.OrderDate,
                Name = order.Name,
                Phone = order.Phone,
                Address = order.Address,
                OrderProducts = order.OrderProducts?.Select(op => new OrderProductDTO
                {
                    ProductId = op.ProductId,
                    Quantity = op.Quantity,
                    ProductName = op.Product?.ProductName
                }).ToList()
            };
        }

        // Chuyển từ CreateOrderRequestDTO sang Order Entity
        public static Order ToEntity(OrderCreateRequestDTO request)
        {
            if (request == null)
                return null;

            return new Order
            {
                CustomerId = request.CustomerId,
                OrderStatusId = request.OrderStatusId,
                PromotionId = request.PromotionId,
                TotalPrice = 0, // Sẽ tính toán sau (hoặc tính ngay tại thời điểm tạo nếu có logic riêng)
                OrderDate = DateTime.UtcNow,
                Name = request.Name,
                Phone = request.Phone,
                Address = request.Address,
                OrderProducts = request.OrderProducts?.Select(op => new OrderProduct
                {
                    ProductId = op.ProductId,
                    Quantity = op.Quantity
                }).ToList()
            };
        }
    }
}
