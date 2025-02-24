using SWP391_CareSkin_BE.DTOs.Requests;
using SWP391_CareSkin_BE.DTOs.Responses;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDTO> CreateOrderAsync(OrderCreateRequestDTO request);
        Task<OrderDTO> GetOrderByIdAsync(int orderId);
        Task<List<OrderDTO>> GetOrdersByCustomerIdAsync(int customerId);
        Task<OrderDTO> UpdateOrderStatusAsync(int orderId, int orderStatusId);
    }
}
