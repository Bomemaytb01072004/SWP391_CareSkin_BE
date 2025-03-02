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
            var promotion = PromotionMapper.ToEntity(request);
            await _promotionRepository.AddPromotionAsync(promotion);

            // Add products to promotion
            if (request.ProductIds != null)
            {
                foreach (var productId in request.ProductIds)
                {
                    await _promotionRepository.AddPromotionProductAsync(promotion.PromotionId, productId);
                }
            }

            // Add customers to promotion
            if (request.CustomerIds != null)
            {
                foreach (var customerId in request.CustomerIds)
                {
                    await _promotionRepository.AddPromotionCustomerAsync(promotion.PromotionId, customerId);
                }
            }

            // Get the complete promotion with relationships
            var createdPromotion = await _promotionRepository.GetPromotionByIdAsync(promotion.PromotionId);
            return PromotionMapper.ToDTO(createdPromotion);
        }

        public async Task<PromotionDTO> UpdatePromotionAsync(int promotionId, PromotionUpdateRequestDTO request)
        {
            var promotion = await _promotionRepository.GetPromotionByIdAsync(promotionId);
            if (promotion == null)
                return null;

            PromotionMapper.UpdateEntity(promotion, request);
            await _promotionRepository.UpdatePromotionAsync(promotion);

            // Update products
            var currentProductIds = promotion.PromotionProducts.Select(pp => pp.ProductId).ToList();
            var productsToAdd = request.ProductIds.Except(currentProductIds);
            var productsToRemove = currentProductIds.Except(request.ProductIds);

            foreach (var productId in productsToAdd)
            {
                await _promotionRepository.AddPromotionProductAsync(promotionId, productId);
            }

            foreach (var productId in productsToRemove)
            {
                await _promotionRepository.RemovePromotionProductAsync(promotionId, productId);
            }

            // Update customers
            var currentCustomerIds = promotion.PromotionCustomers.Select(pc => pc.CustomerId).ToList();
            var customersToAdd = request.CustomerIds.Except(currentCustomerIds);
            var customersToRemove = currentCustomerIds.Except(request.CustomerIds);

            foreach (var customerId in customersToAdd)
            {
                await _promotionRepository.AddPromotionCustomerAsync(promotionId, customerId);
            }

            foreach (var customerId in customersToRemove)
            {
                await _promotionRepository.RemovePromotionCustomerAsync(promotionId, customerId);
            }

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

        public async Task<decimal> CalculateOrderDiscountAsync(int? promotionId, int customerId, decimal orderTotal)
        {
            if (!promotionId.HasValue)
                return 0;

            var promotion = await _promotionRepository.GetPromotionByIdAsync(promotionId.Value);
            if (promotion == null)
                return 0;

            // Check if promotion is active
            var currentDate = DateOnly.FromDateTime(DateTime.UtcNow);
            if (currentDate < promotion.Start_Date || currentDate > promotion.End_Date)
                return 0;

            // Check if promotion applies to this customer
            var customerPromotions = await _promotionRepository.GetPromotionsForCustomerAsync(customerId);
            if (!customerPromotions.Any(p => p.PromotionId == promotionId))
                return 0;

            // Calculate discount
            return orderTotal * (promotion.DiscountPercent / 100);
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
