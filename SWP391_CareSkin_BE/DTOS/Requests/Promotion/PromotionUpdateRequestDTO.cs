using System;

namespace SWP391_CareSkin_BE.DTOS.Requests
{
    public class PromotionUpdateRequestDTO
    {
        public string PromotionName { get; set; }
        public decimal DiscountPercent { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public bool IsActive { get; set; }
        public List<int> ProductIds { get; set; } = new List<int>();
        public List<int> CustomerIds { get; set; } = new List<int>();
    }
}
