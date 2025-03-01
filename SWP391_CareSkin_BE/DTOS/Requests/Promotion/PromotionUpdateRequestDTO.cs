using System;

namespace SWP391_CareSkin_BE.DTOS.Requests
{
    public class PromotionUpdateRequestDTO
    {
        public string PromotionName { get; set; }
        public decimal DiscountPercent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public List<int> ProductIds { get; set; } = new List<int>();
        public List<int> CustomerIds { get; set; } = new List<int>();
    }
}
