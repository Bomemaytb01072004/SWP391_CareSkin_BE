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
            return cartItems.Select(CartMapper.ToDTO).ToList();
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
                return CartMapper.ToDTO(existingCart);
            }

            // Add new cart item if doesn't exist
            var cartEntity = CartMapper.ToEntity(request);
            await _cartRepository.AddCartItemAsync(cartEntity);

            // Get the added cart item with all related data
            var addedCart = await _cartRepository.GetCartItemByIdAsync(cartEntity.CartId);
            return CartMapper.ToDTO(addedCart);
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
            return CartMapper.ToDTO(updatedCart);
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

        public async Task<int> CalculateCartTotalPrice(int customerId)
        {
            var cartItems = await _cartRepository.GetCartItemsByCustomerIdAsync(customerId);
            var total = cartItems.Sum(item => (item.ProductVariation?.Price ?? 0) * item.Quantity);
            return (int)Math.Round(total);
        }
    }
}
