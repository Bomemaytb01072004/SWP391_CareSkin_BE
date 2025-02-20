using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Repositories.Interfaces
{
    public interface ICartRepository
    {
        Task<List<Cart>> GetCartItemsByCustomerIdAsync(int customerId);
        Task AddCartItemAsync(Cart cart);
        Task UpdateCartItemAsync(Cart cart);
        Task RemoveCartItemAsync(int cartId);
        Task<Cart> GetCartItemByIdAsync(int cartId);

    }
}
