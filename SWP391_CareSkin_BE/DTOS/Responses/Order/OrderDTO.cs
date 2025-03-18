namespace SWP391_CareSkin_BE.DTOs.Responses
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int OrderStatusId { get; set; }
        public string OrderStatusName { get; set; }  // Lấy từ OrderStatus
        public int? PromotionId { get; set; }
        public string PromotionName { get; set; } 
        public decimal TotalPrice { get; set; }
        public decimal TotalPriceSale { get; set; }
        public DateOnly OrderDate { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public List<OrderProductDTO> OrderProducts { get; set; }
    }
}
