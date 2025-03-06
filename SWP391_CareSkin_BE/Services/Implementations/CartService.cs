using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.DTOs.Requests;
using SWP391_CareSkin_BE.DTOs.Responses;
using SWP391_CareSkin_BE.Mappers;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Services.Implementations
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly MyDbContext _context;

        public CartService(ICartRepository cartRepository, MyDbContext context)
        {
            _cartRepository = cartRepository;
            _context = context;
        }

        public async Task<List<CartDTO>> GetCartItemsByCustomerIdAsync(int customerId)
        {
            var cartItems = await _cartRepository.GetCartItemsByCustomerIdAsync(customerId);
            var cartDTOs = cartItems.Select(CartMapper.ToDTO).ToList();
            
            // Calculate prices for each cart item
            foreach (var cartDTO in cartDTOs)
            {
                CalculateCartItemPrices(cartDTO);
            }
            
            return cartDTOs;
        }

        public async Task<CartDTO> AddCartItemAsync(CartCreateRequestDTO request)
        {
            // Validate product variation exists
            var productVariation = await _context.ProductVariations
                .FirstOrDefaultAsync(pv => pv.ProductVariationId == request.ProductVariationId);

            if (productVariation == null)
            {
                throw new ArgumentException("Invalid product variation");
            }

            // Check if the same product variation already exists in cart
            var existingCart = await _context.Carts
                .FirstOrDefaultAsync(c => c.CustomerId == request.CustomerId 
                    && c.ProductId == request.ProductId 
                    && c.ProductVariationId == request.ProductVariationId);

            if (existingCart != null)
            {
                // Update quantity if exists
                existingCart.Quantity += request.Quantity;
                await _cartRepository.UpdateCartItemAsync(existingCart);
                
                // Get the updated cart item with all related data
                var updatedCart = await _cartRepository.GetCartItemByIdAsync(existingCart.CartId);
                var cartDTO = CartMapper.ToDTO(updatedCart);
                CalculateCartItemPrices(cartDTO);
                return cartDTO;
            }

            // Add new cart item if doesn't exist
            var cartEntity = CartMapper.ToEntity(request);
            await _cartRepository.AddCartItemAsync(cartEntity);

            // Get the added cart item with all related data
            var addedCart = await _cartRepository.GetCartItemByIdAsync(cartEntity.CartId);
            var addedCartDTO = CartMapper.ToDTO(addedCart);
            CalculateCartItemPrices(addedCartDTO);
            return addedCartDTO;
        }

        public async Task<CartDTO> UpdateCartItemAsync(CartUpdateRequestDTO request)
        {
            // Find existing cart item by customerId and productId
            var existingCart = await _context.Carts
                .FirstOrDefaultAsync(c => c.CustomerId == request.CustomerId && c.ProductId == request.ProductId);

            if (existingCart == null)
                return null;

            // Validate product variation exists
            var productVariation = await _context.ProductVariations
                .FirstOrDefaultAsync(pv => pv.ProductVariationId == request.ProductVariationId);

            if (productVariation == null)
            {
                throw new ArgumentException("Invalid product variation");
            }

            // Update cart item
            existingCart.ProductVariationId = request.ProductVariationId;
            existingCart.Quantity = request.Quantity;
            await _cartRepository.UpdateCartItemAsync(existingCart);

            // Get updated cart item with all related data
            var updatedCart = await _cartRepository.GetCartItemByIdAsync(existingCart.CartId);
            var cartDTO = CartMapper.ToDTO(updatedCart);
            CalculateCartItemPrices(cartDTO);
            return cartDTO;
        }

        public async Task<bool> RemoveCartItemAsync(int cartId)
        {
            var cart = await _cartRepository.GetCartItemByIdAsync(cartId);
            if (cart != null)
            {
                await _cartRepository.RemoveCartItemAsync(cartId);
                return true;
            }
            return false;
        }

        public async Task<decimal> CalculateCartTotalPrice(int customerId)
        {
            // Lấy danh sách cart items của customer
            var cartItems = await _cartRepository.GetCartItemsByCustomerIdAsync(customerId);

            // Sử dụng decimal để tính toán tiền tệ
            decimal total = 0m;
            foreach (var item in cartItems)
            {
                // Giá có thể null => dùng null-coalescing (??) trả về 0m nếu null
                decimal price = item.ProductVariation?.Price ?? 0m;

                // Cộng dồn
                total += price * item.Quantity;
            }

            // Làm tròn về 2 chữ số thập phân (MidpointRounding tuỳ bạn chọn)
            return decimal.Round(total, 2, MidpointRounding.AwayFromZero);
        }
        
        public async Task<decimal> CalculateCartTotalSalePrice(int customerId)
        {
            // Lấy danh sách cart items của customer
            var cartItems = await _cartRepository.GetCartItemsByCustomerIdAsync(customerId);

            // Sử dụng decimal để tính toán tiền tệ
            decimal totalSalePrice = 0m;
            foreach (var item in cartItems)
            {
                decimal originalPrice = item.ProductVariation?.Price ?? 0m;
                decimal salePrice = item.ProductVariation?.SalePrice ?? 0m;
                bool isOnSale = salePrice > 0 && salePrice < originalPrice;
                
                // Sử dụng giá khuyến mãi nếu có, nếu không thì dùng giá gốc
                decimal effectivePrice = isOnSale ? salePrice : originalPrice;

                // Cộng dồn
                totalSalePrice += effectivePrice * item.Quantity;
            }

            // Làm tròn về 2 chữ số thập phân
            return decimal.Round(totalSalePrice, 2, MidpointRounding.AwayFromZero);
        }
        
        public async Task<(decimal TotalPrice, decimal TotalSalePrice)> CalculateCartTotals(int customerId)
        {
            // Lấy danh sách cart items của customer
            var cartItems = await _cartRepository.GetCartItemsByCustomerIdAsync(customerId);

            // Sử dụng decimal để tính toán tiền tệ
            decimal totalPrice = 0m;
            decimal totalSalePrice = 0m;
            
            foreach (var item in cartItems)
            {
                decimal originalPrice = item.ProductVariation?.Price ?? 0m;
                decimal salePrice = item.ProductVariation?.SalePrice ?? 0m;
                bool isOnSale = salePrice > 0 && salePrice < originalPrice;
                
                // Tính tổng giá gốc
                totalPrice += originalPrice * item.Quantity;
                
                // Tính tổng giá sau khuyến mãi
                decimal effectivePrice = isOnSale ? salePrice : originalPrice;
                totalSalePrice += effectivePrice * item.Quantity;
            }

            // Làm tròn về 2 chữ số thập phân
            return (
                decimal.Round(totalPrice, 2, MidpointRounding.AwayFromZero),
                decimal.Round(totalSalePrice, 2, MidpointRounding.AwayFromZero)
            );
        }
        
        // Helper method to calculate prices for a cart item
        private void CalculateCartItemPrices(CartDTO cartDTO)
        {
            if (cartDTO == null) return;
            
            decimal originalPrice = cartDTO.Price;
            decimal salePrice = GetSalePriceForProductVariation(cartDTO.ProductVariationId);
            bool isOnSale = salePrice > 0 && salePrice < originalPrice;
            
            cartDTO.Price = originalPrice;
            cartDTO.TotalPrice = originalPrice * cartDTO.Quantity;
            cartDTO.SalePrice = isOnSale ? salePrice : originalPrice;
            cartDTO.TotalSalePrice = isOnSale ? salePrice * cartDTO.Quantity : originalPrice * cartDTO.Quantity;
            cartDTO.IsOnSale = isOnSale;
        }
        
        // Helper method to get the sale price for a product variation
        private decimal GetSalePriceForProductVariation(int productVariationId)
        {
            var productVariation = _context.ProductVariations
                .FirstOrDefault(pv => pv.ProductVariationId == productVariationId);
                
            return productVariation?.SalePrice ?? 0;
        }
    }
}
