using SWP391_CareSkin_BE.DTOs.Requests;
using SWP391_CareSkin_BE.DTOs.Responses;
using SWP391_CareSkin_BE.Mappers;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<OrderDTO> CreateOrderAsync(OrderCreateRequestDTO request)
        {
            var orderEntity = OrderMapper.ToEntity(request);
            // Optionally: Tính toán TotalPrice ở đây nếu có logic riêng.
            orderEntity.TotalPrice = 0;

            await _orderRepository.AddOrderAsync(orderEntity);
            var createdOrder = await _orderRepository.GetOrderByIdAsync(orderEntity.OrderId);
            return OrderMapper.ToDTO(createdOrder);
        }

        public async Task<OrderDTO> GetOrderByIdAsync(int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            return OrderMapper.ToDTO(order);
        }

        public async Task<List<OrderDTO>> GetOrdersByCustomerIdAsync(int customerId)
        {
            var orders = await _orderRepository.GetOrdersByCustomerIdAsync(customerId);
            return orders.Select(o => OrderMapper.ToDTO(o)).ToList();
        }

        public async Task<OrderDTO> UpdateOrderStatusAsync(int orderId, int orderStatusId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
                return null;
            order.OrderStatusId = orderStatusId;
            await _orderRepository.UpdateOrderAsync(order);
            var updatedOrder = await _orderRepository.GetOrderByIdAsync(orderId);
            return OrderMapper.ToDTO(updatedOrder);
        }
    }
}
