using System;

namespace SWP391_CareSkin_BE.DTOs.Requests.Order
{
    public class OrderUpdateRequestDTO
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int? PromotionId { get; set; }
    }
}
