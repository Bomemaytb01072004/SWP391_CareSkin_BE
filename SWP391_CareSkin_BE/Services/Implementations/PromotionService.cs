using SWP391_CareSkin_BE.DTOS.Requests;
using SWP391_CareSkin_BE.DTOS.Responses;
using SWP391_CareSkin_BE.Mappers;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Services.Implementations
{
    public class PromotionService : IPromotionService
    {
        private readonly IPromotionRepository _promotionRepository;

        public PromotionService(IPromotionRepository promotionRepository)
        {
            _promotionRepository = promotionRepository;
        }

        public async Task<List<PromotionDTO>> GetAllPromotionsAsync()
        {
            var promotions = await _promotionRepository.GetAllPromotionsAsync();
            return promotions.Select(PromotionMapper.ToDTO).ToList();
        }

        public async Task<PromotionDTO> GetPromotionByIdAsync(int promotionId)
        {
            var promotion = await _promotionRepository.GetPromotionByIdAsync(promotionId);
            return PromotionMapper.ToDTO(promotion);
        }

        public async Task<List<PromotionDTO>> GetActivePromotionsAsync()
        {
            var promotions = await _promotionRepository.GetActivePromotionsAsync();
            return promotions.Select(PromotionMapper.ToDTO).ToList();
        }

        public async Task<PromotionDTO> CreatePromotionAsync(PromotionCreateRequestDTO request)
        {
            bool isActive = DateOnly.FromDateTime(DateTime.UtcNow) >= request.StartDate && DateOnly.FromDateTime(DateTime.UtcNow) <= request.EndDate;

            var promotion = PromotionMapper.ToEntity(request, isActive);
            await _promotionRepository.AddPromotionAsync(promotion);

            // Get the complete promotion with relationships
            var createdPromotion = await _promotionRepository.GetPromotionByIdAsync(promotion.PromotionId);
            return PromotionMapper.ToDTO(createdPromotion);
        }

        public async Task<PromotionDTO> UpdatePromotionAsync(int promotionId, PromotionUpdateRequestDTO request)
        {
            var promotion = await _promotionRepository.GetPromotionByIdAsync(promotionId);
            if (promotion == null)
                return null;

            bool isActive = DateOnly.FromDateTime(DateTime.UtcNow) >= request.StartDate && DateOnly.FromDateTime(DateTime.UtcNow) <= request.EndDate;

            PromotionMapper.UpdateEntity(promotion, request, isActive);
            await _promotionRepository.UpdatePromotionAsync(promotion);

            // Get the updated promotion
            var updatedPromotion = await _promotionRepository.GetPromotionByIdAsync(promotionId);
            return PromotionMapper.ToDTO(updatedPromotion);
        }

        public async Task<bool> DeletePromotionAsync(int promotionId)
        {
            var promotion = await _promotionRepository.GetPromotionByIdAsync(promotionId);
            if (promotion == null)
                return false;

            await _promotionRepository.DeletePromotionAsync(promotionId);
            return true;
        }

        public async Task<List<PromotionDTO>> GetPromotionsForCustomerAsync(int customerId)
        {
            var promotions = await _promotionRepository.GetPromotionsForCustomerAsync(customerId);
            return promotions.Select(PromotionMapper.ToDTO).ToList();
        }

        public async Task<List<PromotionDTO>> GetPromotionsForProductAsync(int productId)
        {
            var promotions = await _promotionRepository.GetPromotionsForProductAsync(productId);
            return promotions.Select(PromotionMapper.ToDTO).ToList();
        }
    }
}
