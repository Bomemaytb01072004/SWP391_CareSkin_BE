using SWP391_CareSkin_BE.DTOs.Requests;
using SWP391_CareSkin_BE.DTOs.Responses;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface ICartService
    {
        Task<List<CartDTO>> GetCartItemsByCustomerIdAsync(int customerId);
        Task<CartDTO> AddCartItemAsync(CartCreateRequestDTO request);
        Task<CartDTO> UpdateCartItemAsync(CartUpdateRequestDTO request);
        Task<bool> RemoveCartItemAsync(int cartId);
    }
}
