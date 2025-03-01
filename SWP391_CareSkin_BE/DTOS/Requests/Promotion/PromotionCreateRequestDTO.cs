using System;

namespace SWP391_CareSkin_BE.DTOS.Requests
{
    public class PromotionCreateRequestDTO
    {
        public string PromotionName { get; set; }
        public double DiscountPercent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<int> ProductIds { get; set; } = new List<int>(); // Optional: Products to apply promotion to
        public List<int> CustomerIds { get; set; } = new List<int>(); // Optional: Specific customers for the promotion
    }
}
