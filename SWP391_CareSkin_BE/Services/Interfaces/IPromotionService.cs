using SWP391_CareSkin_BE.DTOS.Requests;
using SWP391_CareSkin_BE.DTOS.Requests.Promotion;
using SWP391_CareSkin_BE.DTOS.Responses;
using SWP391_CareSkin_BE.DTOS.Responses.Promotion;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface IPromotionService
    {
        Task<List<PromotionDTO>> GetAllPromotionsAsync();
        Task<PromotionDTO> GetPromotionByIdAsync(int promotionId);
        Task<List<PromotionDTO>> GetActivePromotionsAsync();
        Task<List<PromotionDTO>> GetActivePromotionsByTypeAsync(PromotionType promotionType);
        Task<PromotionDTO> CreatePromotionAsync(PromotionCreateRequestDTO request);
        Task<PromotionDTO> UpdatePromotionAsync(int promotionId, PromotionUpdateRequestDTO request);
        Task<bool> DeletePromotionAsync(int promotionId);
        Task<PromotionDTO> SetProductDiscountAsync(SetProductDiscountRequestDTO request);
        Task<List<ProductDiscountDTO>> GetProductDiscountsAsync();
        Task<PromotionDTO> UpdateProductDiscountStatusAsync(UpdateProductDiscountStatusDTO request);
        Task<List<ProductDTO>> GetProductsWithDiscountAsync(int promotionId);
        Task<PromotionDTO> DeactivatePromotionAsync(int promotionId);
    }
}
