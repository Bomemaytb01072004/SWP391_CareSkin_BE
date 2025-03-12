using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Repositories.Interfaces
{
    public interface IPromotionRepository
    {
        Task<List<Promotion>> GetAllPromotionsAsync();
        Task<Promotion> GetPromotionByIdAsync(int promotionId);
        Task<List<Promotion>> GetActivePromotionsAsync();
        Task<Promotion> AddPromotionAsync(Promotion promotion);
        Task UpdatePromotionAsync(Promotion promotion);
        Task DeletePromotionAsync(int promotionId);
        Task AddPromotionProductAsync(PromotionProduct promotionProduct);
        Task AddPromotionCustomerAsync(int promotionId, int customerId);
        Task RemovePromotionProductAsync(int promotionId, int productId);
        Task RemovePromotionCustomerAsync(int promotionId, int customerId);
        Task<List<Promotion>> GetPromotionsForCustomerAsync(int customerId);
        Task<List<PromotionProduct>> GetPromotionsForProductAsync(int productId);
    }
}
