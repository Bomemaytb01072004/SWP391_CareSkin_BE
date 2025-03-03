using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.DTOS.Requests.OrderStatus;
using SWP391_CareSkin_BE.DTOS.Responses.OrderStatus;
using SWP391_CareSkin_BE.Mappers;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Services.Implementations
{
    public class OrderStatusService : IOrderStatusService
    {
        private readonly MyDbContext _context;

        public OrderStatusService(MyDbContext context)
        {
            _context = context;
        }

        public async Task<List<OrderStatusDTO>> GetAllOrderStatusesAsync()
        {
            var orderStatuses = await _context.OrderStatuses.ToListAsync();
            return orderStatuses.Select(OrderStatusMapper.ToOrderStatusDTO).ToList();
        }

        public async Task<OrderStatusDTO> GetOrderStatusByIdAsync(int id)
        {
            var orderStatus = await _context.OrderStatuses.FindAsync(id)
                ?? throw new KeyNotFoundException($"Order status with ID {id} not found");

            return OrderStatusMapper.ToOrderStatusDTO(orderStatus);
        }

        public async Task<OrderStatusDTO> CreateOrderStatusAsync(OrderStatusCreateRequestDTO dto)
        {
            var orderStatus = OrderStatusMapper.ToOrderStatus(dto);
            _context.OrderStatuses.Add(orderStatus);
            await _context.SaveChangesAsync();

            return OrderStatusMapper.ToOrderStatusDTO(orderStatus);
        }

        public async Task<OrderStatusDTO> UpdateOrderStatusAsync(int id, OrderStatusUpdateRequestDTO dto)
        {
            var existingStatus = await _context.OrderStatuses.FindAsync(id)
                ?? throw new KeyNotFoundException($"Order status with ID {id} not found");

            // Cập nhật các thuộc tính từ DTO vào entity hiện có
            // Nếu OrderStatusMapper có phương thức UpdateOrderStatusFromDTO, bạn có thể dùng:
            // OrderStatusMapper.UpdateOrderStatusFromDTO(dto, existingStatus);
            // Ngược lại, nếu phương thức ToOrderStatus(dto, existingStatus) cập nhật entity, bạn có thể giữ nguyên:
            var updatedStatus = OrderStatusMapper.ToOrderStatus(dto, existingStatus);

            // Nếu entity đã được EF tracking, việc gọi Update là không cần thiết
            // _context.OrderStatuses.Update(updatedStatus); 

            await _context.SaveChangesAsync();

            return OrderStatusMapper.ToOrderStatusDTO(updatedStatus);
        }

        public async Task DeleteOrderStatusAsync(int id)
        {
            var orderStatus = await _context.OrderStatuses.FindAsync(id)
                ?? throw new KeyNotFoundException($"Order status with ID {id} not found");

            _context.OrderStatuses.Remove(orderStatus);
            await _context.SaveChangesAsync();
        }
    }
}
