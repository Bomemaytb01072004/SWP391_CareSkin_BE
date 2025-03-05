using SWP391_CareSkin_BE.DTOS.Requests;
using SWP391_CareSkin_BE.DTOS.Requests.Promotion;
using SWP391_CareSkin_BE.DTOS.Responses.Promotion;

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
        Task<PromotionDTO> SetProductDiscountAsync(SetProductDiscountRequestDTO request);
        Task<List<ProductDiscountDTO>> GetProductDiscountsAsync();
        Task<PromotionDTO> UpdateProductDiscountStatusAsync(UpdateProductDiscountStatusDTO request);
    }
}
