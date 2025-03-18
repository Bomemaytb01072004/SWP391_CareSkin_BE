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
using SWP391_CareSkin_BE.Repositories.Implementations;
using Microsoft.Extensions.Logging;

namespace SWP391_CareSkin_BE.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly MyDbContext _context;
        private readonly IPromotionRepository _promotionRepository;
        private readonly IProductRepository _productRepository;
        private readonly IEmailService _emailService;
        private readonly ILogger<OrderService> _logger;
        private readonly ICustomerService _customerService;

        public OrderService(
            IOrderRepository orderRepository, 
            MyDbContext context, 
            IPromotionRepository promotionRepository, 
            IProductRepository productRepository,
            IEmailService emailService,
            ILogger<OrderService> logger,
            ICustomerService customerService)
        {
            _orderRepository = orderRepository;
            _context = context;
            _promotionRepository = promotionRepository;
            _productRepository = productRepository;
            _emailService = emailService;
            _logger = logger;
            _customerService = customerService;
        }

        private async Task<decimal> CalculateCartTotalPrice(List<Cart> cartItems, int? promotionId)
        {
            decimal totalPrice = 0;

            foreach (var cartItem in cartItems)
            {
                // Lấy thông tin sản phẩm (bao gồm ProductVariations)
                var product = await _productRepository.GetProductByIdAsync(cartItem.ProductId);
                if (product == null)
                    continue;

                // Lấy variation tương ứng
                var variation = product.ProductVariations.FirstOrDefault(v => v.ProductVariationId == cartItem.ProductVariationId);
                if (variation == null)
                    continue;

                decimal basePrice = variation.Price;

                // Sử dụng SalePrice từ ProductVariation nếu có (> 0)
                decimal finalPrice = variation.SalePrice > 0 ? variation.SalePrice : basePrice;

                decimal itemTotal = finalPrice * cartItem.Quantity;
                totalPrice += itemTotal;
            }

            return decimal.Round(totalPrice, 2);
        }

        private async Task<decimal> CalculateCartTotalPriceSale(List<Cart> cartItems, int? promotionId)
        {
            decimal totalPrice = 0;

            foreach (var cartItem in cartItems)
            {
                // Lấy thông tin sản phẩm (bao gồm ProductVariations)
                var product = await _productRepository.GetProductByIdAsync(cartItem.ProductId);
                if (product == null)
                    continue;

                // Lấy variation tương ứng
                var variation = product.ProductVariations.FirstOrDefault(v => v.ProductVariationId == cartItem.ProductVariationId);
                if (variation == null)
                    continue;

                decimal basePrice = variation.Price;

                // Sử dụng SalePrice từ ProductVariation nếu có (> 0)
                decimal finalPrice = variation.SalePrice > 0 ? variation.SalePrice : basePrice;

                decimal itemTotal = finalPrice * cartItem.Quantity;
                totalPrice += itemTotal;
            }


            // Áp dụng giảm giá đơn hàng nếu có
            if (promotionId.HasValue)
            {
                var promotion = await _context.Promotions
                    .FirstOrDefaultAsync(p => p.PromotionId == promotionId.Value);
                if (promotion != null && promotion.DiscountPercent > 0)
                {
                    decimal discount = decimal.Round(totalPrice * (promotion.DiscountPercent / 100), 2);
                    totalPrice -= discount;
                }
            }

            return decimal.Round(totalPrice, 2);
        }

        public async Task<OrderDTO> CreateOrderAsync(OrderCreateRequestDTO request)
        {
            if (request.SelectedCartItemIds == null || !request.SelectedCartItemIds.Any())
            {
                throw new Exception("Please select products from the shopping cart to place an order.");
            }

            //Check promotion is active or not
            if (request.PromotionId.HasValue)
            {
                bool isPromotionActive = await _context.Promotions
                    .AnyAsync(p => p.PromotionId == request.PromotionId.Value && p.IsActive);

                if (!isPromotionActive)
                {
                    throw new Exception("Selected promotion is not active.");
                }
            }

            // Lấy các cart item dựa theo danh sách CartItemId và đảm bảo thuộc về CustomerId
            var cartItems = await _context.Carts
                .Where(c => request.SelectedCartItemIds.Contains(c.CartId) && c.CustomerId == request.CustomerId)
                .ToListAsync();

            if (!cartItems.Any())
                throw new Exception("No valid products were found in the shopping cart.");

            // Map các thuộc tính chung của Order từ request
            var orderEntity = OrderMapper.ToEntity(request);

            // Tính tổng tiền dựa trên giá của các sản phẩm trong cart
            orderEntity.TotalPrice = await CalculateCartTotalPrice(cartItems, request.PromotionId);
            orderEntity.TotalPriceSale = await CalculateCartTotalPriceSale(cartItems, request.PromotionId);
            // Thêm Order vào database
            await _orderRepository.AddOrderAsync(orderEntity);

            // Tạo OrderProduct từ các cart item
            foreach (var cartItem in cartItems)
            {
                // Lấy thông tin variation để lưu giá và sale price vào OrderProduct
                var variation = await _context.ProductVariations
                    .FirstOrDefaultAsync(pv => pv.ProductVariationId == cartItem.ProductVariationId);
                
                var orderProductEntity = new OrderProduct
                {
                    OrderId = orderEntity.OrderId,
                    ProductId = cartItem.ProductId,
                    ProductVariationId = cartItem.ProductVariationId,
                    Quantity = cartItem.Quantity,
                    Price = variation?.Price ?? 0,
                    SalePrice = variation?.SalePrice > 0 ? variation.SalePrice : (variation?.Price ?? 0)
                };
                _context.OrderProducts.Add(orderProductEntity);
            }

            // Xóa các cart item đã được đặt hàng
            _context.Carts.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            var createdOrder = await _orderRepository.GetOrderByIdAsync(orderEntity.OrderId);
            
            // Lấy thông tin khách hàng để gửi email
            if (createdOrder != null && !string.IsNullOrEmpty(createdOrder.Email))
            {
                try
                {
                    // Gửi email xác nhận đơn hàng
                    await _emailService.SendOrderConfirmationEmailAsync(
                        createdOrder.Email, 
                        createdOrder.OrderId.ToString(), 
                        createdOrder.Name, 
                        createdOrder.TotalPriceSale);
                    
                    _logger.LogInformation("Order confirmation email sent for order {OrderId}", createdOrder.OrderId);
                }
                catch (Exception ex)
                {
                    // Log lỗi nhưng không làm gián đoạn quy trình đặt hàng
                    _logger.LogError(ex, "Error sending order confirmation email for order {OrderId}", createdOrder.OrderId);
                }
            }
            
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
                
                // Get current order products and convert to Cart objects for price calculation
                var orderProducts = await _context.OrderProducts
                    .Where(op => op.OrderId == id)
                    .ToListAsync();

                var cartItems = orderProducts.Select(op => new Cart
                {
                    ProductId = op.ProductId,
                    ProductVariationId = op.ProductVariationId,
                    Quantity = op.Quantity
                }).ToList();

                // Recalculate total price with new promotion
                order.TotalPrice = await CalculateCartTotalPrice(cartItems, request.PromotionId);
                order.TotalPriceSale = await CalculateCartTotalPriceSale(cartItems, request.PromotionId);
                // Update SalePrice in OrderProducts if needed
                foreach (var orderProduct in orderProducts)
                {
                    var variation = await _context.ProductVariations
                        .FirstOrDefaultAsync(pv => pv.ProductVariationId == orderProduct.ProductVariationId);
                    
                    if (variation != null)
                    {
                        orderProduct.Price = variation.Price;
                        orderProduct.SalePrice = variation.SalePrice > 0 ? variation.SalePrice : variation.Price;
                    }
                }
                
                await _context.SaveChangesAsync();
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

        public async Task<List<OrderDTO>> GetOrderHistoryAsync()
        {
            var orders = await _orderRepository.GetOrderHistoryAsync();

            return OrderMapper.ToDTOList(orders);
        }
    }
}
