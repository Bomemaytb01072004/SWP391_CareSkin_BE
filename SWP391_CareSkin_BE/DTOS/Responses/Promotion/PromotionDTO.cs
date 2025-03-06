using System;
using System.Collections.Generic;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.DTOS.Responses.Promotion
{
    public class PromotionDTO
    {
        public int PromotionId { get; set; }
        public string Name { get; set; }
        public decimal DiscountPercent { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public bool IsActive { get; set; }
        public PromotionType PromotionType { get; set; }
        public List<int> ProductIds { get; set; } = new List<int>();
    }
}
