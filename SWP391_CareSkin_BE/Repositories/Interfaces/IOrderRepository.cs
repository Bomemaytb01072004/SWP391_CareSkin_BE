using SWP391_CareSkin_BE.DTOs.Common;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<List<Order>> GetOrdersByCustomerIdAsync(int customerId);
        Task AddOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(int orderId);
        Task<List<Order>> GetOrdersByCustomerAndStatusAsync(int customerId, int statusId);
        Task<PagedResult<Order>> GetOrderHistoryAsync(
            int? customerId,
            int? statusId,
            DateOnly? fromDate,
            DateOnly? toDate,
            int page,
            int pageSize
        );
    }
}
