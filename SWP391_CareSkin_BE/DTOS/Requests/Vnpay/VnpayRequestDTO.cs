namespace SWP391_CareSkin_BE.DTOs.Requests.Vnpay
{
    public class VnpayRequestDTO
    {
        public int OrderId { get; set; }       
        public decimal Amount { get; set; }        
        public string OrderDescription { get; set; }
        public string OrderType { get; set; }

        public string CustomerName { get; set; }
    }
}
