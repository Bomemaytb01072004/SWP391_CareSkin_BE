namespace SWP391_CareSkin_BE.DTOs.Responses.Product
{
    public class PromotionProductDTO
    {
        public int PromotionProductId { get; set; }
        public int ProductId { get; set; }
        public int PromotionId { get; set; }
        public decimal DiscountPercent { get; set; }
        public DateOnly Start_Date { get; set; }
        public DateOnly End_Date { get; set; }
        public decimal SalePrice { get; set; }
    }
}
