using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.DTOS.Responses.Promotion
{
    public class ProductDiscountDTO
    {
        public int ProductId { get; set; }
        public int PromotionId { get; set; }
        public string PromotionName { get; set; }
        public decimal DiscountPercent { get; set; }
        public DateOnly Start_Date { get; set; }
        public DateOnly End_Date { get; set; }
        public bool IsActive { get; set; }
        public PromotionType PromotionType { get; set; }
    }
}
