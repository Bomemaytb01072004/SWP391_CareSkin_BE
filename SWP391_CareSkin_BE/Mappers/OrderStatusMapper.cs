using SWP391_CareSkin_BE.DTOS.Requests.OrderStatus;
using SWP391_CareSkin_BE.DTOS.Responses.OrderStatus;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Mappers
{
    public static class OrderStatusMapper
    {
        public static OrderStatus ToOrderStatus(OrderStatusCreateRequestDTO dto)
        {
            return new OrderStatus
            {
                OrderStatusName = dto.OrderStatusName
            };
        }

        public static OrderStatus ToOrderStatus(OrderStatusUpdateRequestDTO dto, OrderStatus existingStatus)
        {
            existingStatus.OrderStatusName = dto.OrderStatusName;
            return existingStatus;
        }

        public static OrderStatusDTO ToOrderStatusDTO(OrderStatus orderStatus)
        {
            return new OrderStatusDTO
            {
                OrderStatusId = orderStatus.OrderStatusId,
                OrderStatusName = orderStatus.OrderStatusName
            };
        }
    }
}
