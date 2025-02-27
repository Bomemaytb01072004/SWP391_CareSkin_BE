namespace SWP391_CareSkin_BE.DTOs.Requests
{
    public class CartCreateRequestDTO
    {
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
