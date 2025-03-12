using SWP391_CareSkin_BE.DTOS.Requests.OrderStatus;
using SWP391_CareSkin_BE.DTOS.Responses.OrderStatus;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface IOrderStatusService
    {
        Task<List<OrderStatusDTO>> GetAllOrderStatusesAsync();
        Task<OrderStatusDTO> GetOrderStatusByIdAsync(int id);
        Task<OrderStatusDTO> CreateOrderStatusAsync(OrderStatusCreateRequestDTO dto);
        Task<OrderStatusDTO> UpdateOrderStatusAsync(int id, OrderStatusUpdateRequestDTO dto);
        Task DeleteOrderStatusAsync(int id);
    }
}
