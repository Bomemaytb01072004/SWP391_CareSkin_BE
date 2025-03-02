namespace SWP391_CareSkin_BE.DTOs.Requests
{
    public class OrderCreateRequestDTO
    {
        public int CustomerId { get; set; }
        public int OrderStatusId { get; set; }
        public int? PromotionId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public List<int>? SelectedCartItemIds { get; set; }
        public List<OrderProductRequestDTO>? OrderProducts { get; set; }
    }
}
