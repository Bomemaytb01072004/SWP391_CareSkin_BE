using SWP391_CareSkin_BE.DTOs.Common;
using SWP391_CareSkin_BE.DTOs.Requests;
using SWP391_CareSkin_BE.DTOs.Requests.Order;
using SWP391_CareSkin_BE.DTOs.Responses;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDTO> CreateOrderAsync(OrderCreateRequestDTO request);
        Task<OrderDTO> GetOrderByIdAsync(int orderId);
        Task<List<OrderDTO>> GetOrdersByCustomerIdAsync(int customerId);
        Task<OrderDTO> UpdateOrderStatusAsync(int orderId, int orderStatusId);
        Task<OrderDTO> UpdateOrderAsync(int id, OrderUpdateRequestDTO request);
        Task<bool> CancelOrderAsync(int id);
        Task<List<OrderDTO>> GetOrdersByCustomerAndStatusAsync(int customerId, int statusId);
        Task<PagedResult<OrderDTO>> GetOrderHistoryAsync(OrderHistoryRequestDTO request);
    }
}
