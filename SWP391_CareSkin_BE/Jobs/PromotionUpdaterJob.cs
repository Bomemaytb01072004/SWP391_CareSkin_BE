using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Jobs
{
    public class PromotionUpdaterJob
    {
        private readonly MyDbContext _dbContext;
        private readonly ILogger<PromotionUpdaterJob> _logger;

        public PromotionUpdaterJob(MyDbContext dbContext, ILogger<PromotionUpdaterJob> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task UpdatePromotionStatusesAsync()
        {
            // Sử dụng thời gian local của máy
            var currentDate = DateOnly.FromDateTime(DateTime.Now);
            _logger.LogInformation($"Running promotion status update job on {DateTime.Now}. Current date: {currentDate}");

            // Update promotion statuses
            var promotions = await _dbContext.Promotions
                .AsSplitQuery() // Add split query behavior to optimize performance
                .ToListAsync();
            int updatedPromotions = 0;

            foreach (var promotion in promotions)
            {
                bool shouldBeActive = currentDate >= promotion.Start_Date && currentDate <= promotion.End_Date;
                
                if (promotion.IsActive != shouldBeActive)
                {
                    promotion.IsActive = shouldBeActive;
                    updatedPromotions++;
                    _logger.LogInformation($"Promotion {promotion.PromotionId} '{promotion.PromotionName}' updated: IsActive = {shouldBeActive}");
                }
            }

            if (updatedPromotions > 0)
            {
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Updated status for {updatedPromotions} promotions");
            }
            else
            {
                _logger.LogInformation("No promotion status updates needed");
            }

            // Update PromotionProduct statuses based on their parent Promotion status
            await UpdatePromotionProductStatusesAsync();
        }

        // New method to force update all promotions
        public async Task ForceUpdateAllPromotionsAsync()
        {
            // Sử dụng thời gian local của máy
            var currentDate = DateOnly.FromDateTime(DateTime.Now);
            _logger.LogInformation($"Force updating all promotions on {DateTime.Now}. Current date: {currentDate}");

            // Get all promotions
            var promotions = await _dbContext.Promotions
                .AsSplitQuery()
                .ToListAsync();
            int updatedPromotions = 0;

            foreach (var promotion in promotions)
            {
                bool shouldBeActive = currentDate >= promotion.Start_Date && currentDate <= promotion.End_Date;
                
                // Force update regardless of current state
                if (promotion.IsActive != shouldBeActive)
                {
                    promotion.IsActive = shouldBeActive;
                    updatedPromotions++;
                    _logger.LogInformation($"Force updated: Promotion {promotion.PromotionId} '{promotion.PromotionName}' to IsActive = {shouldBeActive}");
                }
                else
                {
                    _logger.LogInformation($"Promotion {promotion.PromotionId} '{promotion.PromotionName}' already has correct status: IsActive = {shouldBeActive}");
                }
            }

            if (updatedPromotions > 0)
            {
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Force updated status for {updatedPromotions} promotions");
            }
            else
            {
                _logger.LogInformation("No promotion status updates needed during force update");
            }

            // Update all promotion products
            await ForceUpdateAllPromotionProductsAsync();
        }

        public async Task UpdatePromotionProductStatusesAsync()
        {
            // Get only active promotion products with their related promotions
            var activePromotionProducts = await _dbContext.PromotionProducts
                .Include(pp => pp.Promotion)
                .Where(pp => pp.IsActive) // Only get currently active promotion products
                .AsSplitQuery() // Add split query behavior to optimize performance
                .ToListAsync();

            int updatedPromotionProducts = 0;
            // Sử dụng thời gian local của máy
            var currentDate = DateOnly.FromDateTime(DateTime.Now);

            foreach (var promotionProduct in activePromotionProducts)
            {
                // Check if the parent promotion is still active and within its date range
                var promotion = promotionProduct.Promotion;
                bool promotionIsActive = promotion.IsActive && 
                                        currentDate >= promotion.Start_Date && 
                                        currentDate <= promotion.End_Date;
                
                // Only deactivate promotion products if their promotion is no longer active
                // We don't reactivate manually deactivated promotion products
                if (!promotionIsActive && promotionProduct.IsActive)
                {
                    promotionProduct.IsActive = false;
                    updatedPromotionProducts++;
                    _logger.LogInformation($"PromotionProduct {promotionProduct.PromotionProductId} deactivated because its promotion is no longer active or out of date range");

                    // Reset the SalePrice for all variations of this product
                    await ResetProductVariationSalePricesAsync(promotionProduct.ProductId);
                }
            }

            if (updatedPromotionProducts > 0)
            {
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Deactivated {updatedPromotionProducts} promotion products due to expired or inactive promotions");
            }
            else
            {
                _logger.LogInformation("No promotion product status updates needed");
            }
        }

        // New method to force update all promotion products
        public async Task ForceUpdateAllPromotionProductsAsync()
        {
            // Get all promotion products with their related promotions
            var promotionProducts = await _dbContext.PromotionProducts
                .Include(pp => pp.Promotion)
                .AsSplitQuery()
                .ToListAsync();

            int updatedPromotionProducts = 0;
            // Sử dụng thời gian local của máy
            var currentDate = DateOnly.FromDateTime(DateTime.Now);

            foreach (var promotionProduct in promotionProducts)
            {
                // Check if the parent promotion is active and within its date range
                var promotion = promotionProduct.Promotion;
                bool promotionIsActive = promotion.IsActive && 
                                        currentDate >= promotion.Start_Date && 
                                        currentDate <= promotion.End_Date;
                
                // Update promotion product status based on promotion status
                if (promotionIsActive != promotionProduct.IsActive)
                {
                    promotionProduct.IsActive = promotionIsActive;
                    updatedPromotionProducts++;
                    _logger.LogInformation($"Force updated: PromotionProduct {promotionProduct.PromotionProductId} to IsActive = {promotionIsActive}");
                    
                    // If deactivating, reset the SalePrice for all variations of this product
                    if (!promotionIsActive)
                    {
                        await ResetProductVariationSalePricesAsync(promotionProduct.ProductId);
                    }
                }
            }

            if (updatedPromotionProducts > 0)
            {
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Force updated {updatedPromotionProducts} promotion products");
            }
            else
            {
                _logger.LogInformation("No promotion product status updates needed during force update");
            }
        }

        private async Task ResetProductVariationSalePricesAsync(int productId)
        {
            // Get all variations for the product
            var variations = await _dbContext.ProductVariations
                .Where(pv => pv.ProductId == productId)
                .AsSplitQuery() // Add split query behavior to optimize performance
                .ToListAsync();

            foreach (var variation in variations)
            {
                if (variation.SalePrice != 0)
                {
                    variation.SalePrice = 0;
                    _logger.LogInformation($"Reset SalePrice for ProductVariation {variation.ProductVariationId} of Product {productId}");
                }
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
