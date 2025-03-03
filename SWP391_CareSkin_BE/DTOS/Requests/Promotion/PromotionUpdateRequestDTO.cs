using System;

namespace SWP391_CareSkin_BE.DTOS.Requests
{
    public class PromotionUpdateRequestDTO
    {
        public string PromotionName { get; set; }
        public decimal DiscountPercent { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
    }
}
