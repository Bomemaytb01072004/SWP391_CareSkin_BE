namespace SWP391_CareSkin_BE.DTOs.Requests
{
    public class CartUpdateRequestDTO
    {
        public int CartId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
