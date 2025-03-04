namespace SWP391_CareSkin_BE.DTOs.Requests.Vnpay
{
    public class VnpayRequestDTO
    {
        public string OrderId { get; set; } 
        public decimal Amount { get; set; }  
        public string ReturnUrl { get; set; }
        public string IpAddress { get; set; }
    }
}
