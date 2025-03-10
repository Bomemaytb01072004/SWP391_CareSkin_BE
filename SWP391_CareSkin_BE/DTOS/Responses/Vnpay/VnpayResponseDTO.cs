namespace SWP391_CareSkin_BE.DTOs.Responses.Vnpay
{
    public class VnpayResponseDTO
    {
        public string OrderDescription { get; set; }
        public string TransactionId { get; set; }
        public int OrderId { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentId { get; set; }
        public bool Success { get; set; }
        public string Token { get; set; }
        public string VnPayResponseCode { get; set; }
    }
}