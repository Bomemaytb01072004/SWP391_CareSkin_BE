namespace SWP391_CareSkin_BE.DTOs.Requests
{
    public class MomoRequest
    {
        public long Amount { get; set; }
        public string OrderId { get; set; }
        public string ReturnUrl { get; set; }
        public string NotifyUrl { get; set; }
    }
}
