using SWP391_CareSkin_BE.DTOS.Requests;
using SWP391_CareSkin_BE.DTOS.Responses;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Mappers
{
    public static class PromotionMapper
    {
        public static PromotionDTO ToDTO(Promotion promotion)
        {
            if (promotion == null) return null;

            return new PromotionDTO
            {
                PromotionId = promotion.PromotionId,
                Name = promotion.PromotionName,
                DiscountPercent = promotion.DiscountPercent,
                StartDate = promotion.Start_Date,
                EndDate = promotion.End_Date,
                IsActive = DateTime.Now >= promotion.Start_Date && DateTime.Now <= promotion.End_Date,
                ProductIds = promotion.PromotionProducts?.Select(pp => pp.ProductId).ToList() ?? new List<int>(),
                CustomerIds = promotion.PromotionCustomers?.Select(pc => pc.CustomerId).ToList() ?? new List<int>()
            };
        }

        public static Promotion ToEntity(PromotionCreateRequestDTO dto)
        {
            return new Promotion
            {
                PromotionName = dto.PromotionName,
                DiscountPercent = dto.DiscountPercent,
                Start_Date = dto.StartDate,
                End_Date = dto.EndDate
            };
        }

        public static void UpdateEntity(Promotion promotion, PromotionUpdateRequestDTO dto)
        {
            promotion.PromotionName = dto.PromotionName;
            promotion.DiscountPercent = dto.DiscountPercent;
            promotion.Start_Date = dto.StartDate;
            promotion.End_Date = dto.EndDate;
        }
    }
}
