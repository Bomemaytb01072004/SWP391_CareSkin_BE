using SWP391_CareSkin_BE.DTOs.Requests;
using SWP391_CareSkin_BE.DTOs.Responses;
using SWP391_CareSkin_BE.Mappers;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Services.Implementations
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<List<CartDTO>> GetCartItemsByCustomerIdAsync(int customerId)
        {
            var cartItems = await _cartRepository.GetCartItemsByCustomerIdAsync(customerId);
            return cartItems.Select(CartMapper.ToDTO).ToList();
        }

        public async Task<CartDTO> AddCartItemAsync(CartCreateRequestDTO request)
        {
            // Chuyển từ DTO sang Entity
            var cartEntity = CartMapper.ToEntity(request);
            await _cartRepository.AddCartItemAsync(cartEntity);

            // Lấy lại mục vừa được thêm (bao gồm thông tin Product nếu cần)
            var addedCart = await _cartRepository.GetCartItemByIdAsync(cartEntity.CartId);
            return CartMapper.ToDTO(addedCart);
        }

        public async Task<CartDTO> UpdateCartItemAsync(CartUpdateRequestDTO request)
        {
            // Lấy mục giỏ hàng hiện có
            var existingCart = await _cartRepository.GetCartItemByIdAsync(request.CartId);
            if (existingCart == null)
                return null;

            // Cập nhật số lượng
            existingCart.Quantity = request.Quantity;
            await _cartRepository.UpdateCartItemAsync(existingCart);

            var updatedCart = await _cartRepository.GetCartItemByIdAsync(request.CartId);
            return CartMapper.ToDTO(updatedCart);
        }

        public async Task<bool> RemoveCartItemAsync(int cartId)
        {
            await _cartRepository.RemoveCartItemAsync(cartId);
            return true;
        }
    }
}
