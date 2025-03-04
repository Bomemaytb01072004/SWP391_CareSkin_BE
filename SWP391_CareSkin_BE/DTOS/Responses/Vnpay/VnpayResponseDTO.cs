namespace SWP391_CareSkin_BE.DTOs.Responses.Vnpay
{
    public class VnpayResponseDTO
    {
        public string PaymentUrl { get; set; }
        public string OrderId { get; set; }
        public decimal Amount { get; set; }

        public string Status { get; set; }
    }
}
