namespace SWP391_CareSkin_BE.DTOs.Responses
{
    public class MomoResponse
    {
        public string OrderId { get; set; } 
        public string RequestId { get; set; }
        public long Amount { get; set; }
        public int ResultCode { get; set; }
        public string Message { get; set; }
        public string PayUrl { get; set; }
        public string Status { get; set; }
    }
}
