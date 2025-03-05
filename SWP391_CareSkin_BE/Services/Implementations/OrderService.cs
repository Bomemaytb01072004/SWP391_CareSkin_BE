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

namespace SWP391_CareSkin_BE.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly MyDbContext _context;
        private readonly IPromotionRepository _promotionRepository;
        private readonly IProductRepository _productRepository;

        public OrderService(IOrderRepository orderRepository, MyDbContext context, IPromotionRepository promotionRepository, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _context = context;
            _promotionRepository = promotionRepository;
            _productRepository = productRepository;
        }

        //private async Task<decimal> CalculateCartTotalPrice(List<Cart> cartItems, int? promotionId)
        //{
        //    decimal totalPrice = 0;

        //    foreach (var cartItem in cartItems)
        //    {
        //        var productVariation = await _context.ProductVariations
        //            .FirstOrDefaultAsync(pv => pv.ProductVariationId == cartItem.ProductVariationId);

        //        if (productVariation != null)
        //        {
        //            totalPrice += productVariation.Price * cartItem.Quantity;
        //        }
        //    }

        //    if (promotionId.HasValue)
        //    {
        //        var promotion = await _context.Promotions.FirstOrDefaultAsync(p => p.PromotionId == promotionId.Value);

        //        if (promotion != null && promotion.DiscountPercent > 0)
        //        {
        //            decimal discount = decimal.Round(totalPrice * (promotion.DiscountPercent / 100.0m), 2);
        //            totalPrice -= discount;
        //        }
        //    }

        //    return decimal.Round(totalPrice, 2);
        //}

        //private async Task<decimal> CalculateCartTotalPrice(List<Cart> cartItems, int? promotionId)
        //{
        //    decimal totalPrice = 0;

        //    foreach (var cartItem in cartItems)
        //    {
        //        // Lấy thông tin sản phẩm (bao gồm PromotionProducts và ProductVariations)
        //        var product = await _productRepository.GetProductByIdAsync(cartItem.ProductId);
        //        if (product == null)
        //            continue;

        //        // Lấy variation tương ứng (giá gốc)
        //        var variation = product.ProductVariations.FirstOrDefault(v => v.ProductVariationId == cartItem.ProductVariationId);
        //        decimal basePrice = variation != null ? variation.Price : 0;
        //        decimal finalPrice = basePrice;

        //        // Nếu sản phẩm có giảm giá riêng (cấp sản phẩm), lấy SalePrice từ PromotionProduct active
        //        if (product.PromotionProducts != null && product.PromotionProducts.Any())
        //        {
        //            var activePromotion = product.PromotionProducts
        //                .FirstOrDefault(pp => pp.Promotion != null
        //                                   && pp.Promotion.IsActive
        //                                   && pp.Promotion.DiscountPercent != 0);
        //            if (activePromotion != null)
        //            {
        //                finalPrice = activePromotion.SalePrice;
        //            }
        //        }

        //        totalPrice += finalPrice * cartItem.Quantity;
        //    }

        //    // Áp dụng giảm giá tổng đơn hàng nếu có Promotion và DiscountPercent > 0
        //    if (promotionId.HasValue)
        //    {
        //        var promotion = await _context.Promotions
        //            .FirstOrDefaultAsync(p => p.PromotionId == promotionId.Value);
        //        if (promotion != null && promotion.DiscountPercent > 0)
        //        {
        //            decimal discount = decimal.Round(totalPrice * (promotion.DiscountPercent / 100), 2);
        //            totalPrice -= discount;
        //        }
        //    }

        //    return decimal.Round(totalPrice, 2);
        //}

        private async Task<decimal> CalculateCartTotalPrice(List<Cart> cartItems, int? promotionId)
        {
            decimal totalPrice = 0;

            foreach (var cartItem in cartItems)
            {
                // Lấy thông tin sản phẩm (bao gồm PromotionProducts và ProductVariations)
                var product = await _productRepository.GetProductByIdAsync(cartItem.ProductId);
                if (product == null)
                    continue;

                // Lấy variation tương ứng (giá gốc)
                var variation = product.ProductVariations.FirstOrDefault(v => v.ProductVariationId == cartItem.ProductVariationId);
                decimal basePrice = variation != null ? variation.Price : 0;
                Console.WriteLine($"CartItem ID: {cartItem.CartId} - BasePrice: {basePrice}");

                decimal finalPrice = basePrice;

                // Kiểm tra nếu sản phẩm có được giảm giá riêng (cấp sản phẩm)
                if (product.PromotionProducts != null && product.PromotionProducts.Any())
                {
                    var activePromotion = product.PromotionProducts
                        .FirstOrDefault(pp => pp.Promotion != null
                                           && pp.Promotion.IsActive
                                           && pp.Promotion.DiscountPercent != 0);
                    if (activePromotion != null)
                    {
                        finalPrice = activePromotion.SalePrice;
                        Console.WriteLine($"CartItem ID: {cartItem.CartId} - ActivePromotion SalePrice: {finalPrice}");
                    }
                }

                decimal itemTotal = finalPrice * cartItem.Quantity;
                Console.WriteLine($"CartItem ID: {cartItem.CartId} - Quantity: {cartItem.Quantity}, ItemTotal: {itemTotal}");
                totalPrice += itemTotal;
            }

            Console.WriteLine($"Total before order-level discount: {totalPrice}");

            // Áp dụng giảm giá đơn hàng nếu có
            if (promotionId.HasValue)
            {
                var promotion = await _context.Promotions
                    .FirstOrDefaultAsync(p => p.PromotionId == promotionId.Value);
                if (promotion != null && promotion.DiscountPercent > 0)
                {
                    decimal discount = decimal.Round(totalPrice * (promotion.DiscountPercent / 100), 2);
                    Console.WriteLine($"Order-level discount: {discount} (Promotion DiscountPercent: {promotion.DiscountPercent})");
                    totalPrice -= discount;
                }
            }

            Console.WriteLine($"Total after order-level discount: {totalPrice}");
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

            // Thêm Order vào database
            await _orderRepository.AddOrderAsync(orderEntity);

            // Tạo OrderProduct từ các cart item
            foreach (var cartItem in cartItems)
            {
                var orderProductEntity = new OrderProduct
                {
                    OrderId = orderEntity.OrderId,
                    ProductId = cartItem.ProductId,
                    ProductVariationId = cartItem.ProductVariationId,
                    Quantity = cartItem.Quantity
                };
                _context.OrderProducts.Add(orderProductEntity);
            }

            // Xóa các cart item đã được đặt hàng
            _context.Carts.RemoveRange(cartItems);
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
                order.TotalPrice = await CalculateCartTotalPrice(orderProducts.Select(op => new Cart
                {
                    ProductVariationId = op.ProductId,
                    Quantity = op.Quantity
                }).ToList(), request.PromotionId);
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
