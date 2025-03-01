using SWP391_CareSkin_BE.DTOS.Requests;
using SWP391_CareSkin_BE.DTOS.Responses;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface IPromotionService
    {
        Task<List<PromotionDTO>> GetAllPromotionsAsync();
        Task<PromotionDTO> GetPromotionByIdAsync(int promotionId);
        Task<List<PromotionDTO>> GetActivePromotionsAsync();
        Task<PromotionDTO> CreatePromotionAsync(PromotionCreateRequestDTO request);
        Task<PromotionDTO> UpdatePromotionAsync(int promotionId, PromotionUpdateRequestDTO request);
        Task<bool> DeletePromotionAsync(int promotionId);
        Task<decimal> CalculateOrderDiscountAsync(int? promotionId, int customerId, decimal orderTotal);
        Task<List<PromotionDTO>> GetPromotionsForCustomerAsync(int customerId);
        Task<List<PromotionDTO>> GetPromotionsForProductAsync(int productId);
    }
}
