using SWP391_CareSkin_BE.DTOs.Common;
using SWP391_CareSkin_BE.DTOs.Requests;
using SWP391_CareSkin_BE.DTOs.Requests.Order;
using SWP391_CareSkin_BE.DTOs.Responses;
using SWP391_CareSkin_BE.Mappers;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using SWP391_CareSkin_BE.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;

namespace SWP391_CareSkin_BE.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly MyDbContext _context;

        public OrderService(IOrderRepository orderRepository, MyDbContext context)
        {
            _orderRepository = orderRepository;
            _context = context;
        }

        private async Task<int> CalculateOrderTotalPrice(List<OrderProductRequestDTO> orderProducts, int? promotionId)
        {
            int totalPrice = 0;

            foreach (var orderProduct in orderProducts)
            {
                var productVariation = await _context.ProductVariations
                    .FirstOrDefaultAsync(pv => pv.ProductVariationId == orderProduct.ProductVariationId);

                if (productVariation != null)
                {
                    totalPrice += productVariation.Price * orderProduct.Quantity;
                }
            }

            if (promotionId.HasValue)
            {
                var promotion = await _context.Promotions.FirstOrDefaultAsync(p => p.PromotionId == promotionId.Value);

                if (promotion != null && promotion.DiscountPercent > 0)
                {
                    decimal discount = Math.Round(totalPrice * (promotion.DiscountPercent / 100m));
                    totalPrice -= (int)discount;
                }
            }

            return totalPrice;
        }


        public async Task<OrderDTO> CreateOrderAsync(OrderCreateRequestDTO request)
        {
            var orderEntity = OrderMapper.ToEntity(request);
            
            // Calculate total price based on products and promotion
            orderEntity.TotalPrice = await CalculateOrderTotalPrice(request.OrderProducts, request.PromotionId);

            await _orderRepository.AddOrderAsync(orderEntity);
            
            // Add order products
            foreach (var orderProduct in request.OrderProducts)
            {
                var orderProductEntity = new OrderProduct
                {
                    OrderId = orderEntity.OrderId,
                    ProductId = orderProduct.ProductId,
                    Quantity = orderProduct.Quantity
                };
                _context.OrderProducts.Add(orderProductEntity);
            }
            await _context.SaveChangesAsync();

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

        public async Task<OrderDTO> UpdateOrderAsync(int id, OrderUpdateRequestDTO request)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order == null) return null;

            // Only allow updates if order is in "New" status
            if (order.OrderStatusId != 1)
                throw new InvalidOperationException("Can only update orders in 'New' status");

            order.Name = request.Name;
            order.Phone = request.Phone;
            order.Address = request.Address;

            // If promotion is changed, recalculate total price
            if (request.PromotionId.HasValue && request.PromotionId != order.PromotionId)
            {
                order.PromotionId = request.PromotionId.Value;
                // Get current order products
                var orderProducts = await _context.OrderProducts
                    .Where(op => op.OrderId == id)
                    .Select(op => new OrderProductRequestDTO
                    {
                        ProductId = op.ProductId,
                        Quantity = op.Quantity
                    })
                    .ToListAsync();

                // Recalculate total price with new promotion
                order.TotalPrice = await CalculateOrderTotalPrice(orderProducts, request.PromotionId);
            }

            await _orderRepository.UpdateOrderAsync(order);
            var updatedOrder = await _orderRepository.GetOrderByIdAsync(id);
            return OrderMapper.ToDTO(updatedOrder);
        }

        public async Task<bool> CancelOrderAsync(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order == null) return false;

            // Can only cancel orders in "New" status
            if (order.OrderStatusId != 1)
                return false;

            order.OrderStatusId = 4; // Cancelled status
            await _orderRepository.UpdateOrderAsync(order);
            return true;
        }

        public async Task<List<OrderDTO>> GetOrdersByCustomerAndStatusAsync(int customerId, int statusId)
        {
            var orders = await _orderRepository.GetOrdersByCustomerAndStatusAsync(customerId, statusId);
            return orders.Select(o => OrderMapper.ToDTO(o)).ToList();
        }

        public async Task<PagedResult<OrderDTO>> GetOrderHistoryAsync(OrderHistoryRequestDTO request)
        {
            var orders = await _orderRepository.GetOrderHistoryAsync(
                request.CustomerId,
                request.StatusId,
                request.FromDate,
                request.ToDate,
                request.Page,
                request.PageSize
            );
            
            return new PagedResult<OrderDTO>
            {
                Items = orders.Items.Select(o => OrderMapper.ToDTO(o)).ToList(),
                TotalItems = orders.TotalItems,
                Page = orders.Page,
                PageSize = orders.PageSize
            };
        }
    }
}
