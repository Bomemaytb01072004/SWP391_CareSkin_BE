using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.DTOs.Common;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;

namespace SWP391_CareSkin_BE.Repositories.Implementations
{
    public class OrderRepository : IOrderRepository
    {
        private readonly MyDbContext _context;
        public OrderRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderStatus)
                .Include(o => o.Promotion)
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task<List<Order>> GetOrdersByCustomerIdAsync(int customerId)
        {
            return await _context.Orders
                .Include(o => o.OrderStatus)
                .Include(o => o.Promotion)
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                .Where(o => o.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task AddOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(int orderId)
        {
            var order = await GetOrderByIdAsync(orderId);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Order>> GetOrdersByCustomerAndStatusAsync(int customerId, int statusId)
        {
            return await _context.Orders
                .Include(o => o.OrderStatus)
                .Include(o => o.Promotion)
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                .Where(o => o.CustomerId == customerId && o.OrderStatusId == statusId)
                .ToListAsync();
        }

        public async Task<PagedResult<Order>> GetOrderHistoryAsync(
            int? customerId,
            int? statusId,
            DateOnly? fromDate,
            DateOnly? toDate,
            int page,
            int pageSize)
        {
            var query = _context.Orders
                .Include(o => o.OrderStatus)
                .Include(o => o.Promotion)
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                .AsQueryable();

            if (customerId.HasValue)
                query = query.Where(o => o.CustomerId == customerId.Value);

            if (statusId.HasValue)
                query = query.Where(o => o.OrderStatusId == statusId.Value);

            if (fromDate.HasValue)
                query = query.Where(o => o.OrderDate >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(o => o.OrderDate <= toDate.Value);

            var totalItems = await query.CountAsync();
            var items = await query
                .OrderByDescending(o => o.OrderDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Order>
            {
                Items = items,
                TotalItems = totalItems,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}
