using SWP391_CareSkin_BE.DTOS.Requests;
using SWP391_CareSkin_BE.DTOS.Requests.Promotion;
using SWP391_CareSkin_BE.DTOS.Responses.Promotion;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Mappers
{
    public static class PromotionMapper
    {
        public static PromotionDTO ToDTO(Promotion entity)
        {
            if (entity == null) return null;

            return new PromotionDTO
            {
                PromotionId = entity.PromotionId,
                Name = entity.PromotionName,
                DiscountPercent = entity.DiscountPercent,
                StartDate = entity.Start_Date,
                EndDate = entity.End_Date,
                IsActive = entity.IsActive,
                PromotionType = entity.PromotionType,
                ProductIds = entity.PromotionProducts?.Select(pp => pp.ProductId).ToList() ?? new List<int>()
            };
        }

        public static Promotion ToEntity(PromotionCreateRequestDTO dto, bool isActive)
        {
            return new Promotion
            {
                PromotionName = dto.PromotionName,
                DiscountPercent = dto.DiscountPercent,
                Start_Date = dto.StartDate,
                End_Date = dto.EndDate,
                IsActive = isActive,
                PromotionType = dto.PromotionType
            };
        }

        public static void UpdateEntity(Promotion promotion, PromotionUpdateRequestDTO dto, bool isActive)
        {
            promotion.PromotionName = dto.PromotionName;
            promotion.DiscountPercent = dto.DiscountPercent;
            promotion.Start_Date = dto.StartDate;
            promotion.End_Date = dto.EndDate;
            promotion.IsActive = isActive;
            promotion.PromotionType = dto.PromotionType;
        }

        public static PromotionProduct ToEntity(SetProductDiscountRequestDTO dto)
        {
            if (dto == null) return null;

            return new PromotionProduct
            {
                ProductId = dto.ProductId,
                PromotionId = dto.PromotionId,
                IsActive = true
            };
        }

        public static ProductDiscountDTO ToProductDiscountDTO(PromotionProduct promotionProduct, Promotion promotion)
        {
            if (promotionProduct == null || promotion == null) return null;

            // Get the first product variation to get a representative SalePrice
            // This is for backward compatibility with the DTO
            var product = promotionProduct.Product;
            decimal salePrice = 0;
            
            if (product != null && product.ProductVariations != null && product.ProductVariations.Any())
            {
                var firstVariation = product.ProductVariations.FirstOrDefault();
                if (firstVariation != null)
                {
                    salePrice = firstVariation.SalePrice;
                }
            }

            return new ProductDiscountDTO
            {
                ProductId = promotionProduct.ProductId,
                PromotionId = promotionProduct.PromotionId,
                PromotionName = promotion.PromotionName,
                DiscountPercent = promotion.DiscountPercent,
                Start_Date = promotion.Start_Date,
                End_Date = promotion.End_Date,
                IsActive = promotionProduct.IsActive,
                PromotionType = promotion.PromotionType
            };
        }

        public static PromotionProduct ToEntity(UpdateProductDiscountStatusDTO dto)
        {
            if (dto == null) return null;

            return new PromotionProduct
            {
                ProductId = dto.ProductId,
                PromotionId = dto.PromotionId,
                IsActive = dto.IsActive
            };
        }
    }
}
