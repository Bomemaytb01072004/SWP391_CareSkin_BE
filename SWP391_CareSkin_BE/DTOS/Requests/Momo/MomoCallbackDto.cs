using System.Text.Json.Serialization;

namespace SWP391_CareSkin_BE.DTOS.Requests.Momo
{
    /// <summary>
    /// DTO for Momo callback data (IPN)
    /// </summary>
    public class MomoCallbackDto
    {
        [JsonPropertyName("partnerCode")]
        public string PartnerCode { get; set; } = string.Empty;
        
        [JsonPropertyName("orderId")]
        public string OrderId { get; set; } = string.Empty;
        
        [JsonPropertyName("requestId")]
        public string RequestId { get; set; } = string.Empty;
        
        [JsonPropertyName("amount")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public long Amount { get; set; }
        
        [JsonPropertyName("orderInfo")]
        public string OrderInfo { get; set; } = string.Empty;
        
        [JsonPropertyName("orderType")]
        public string OrderType { get; set; } = string.Empty;
        
        [JsonPropertyName("transId")]
        public string TransId { get; set; } = string.Empty;
        
        [JsonPropertyName("resultCode")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public int ResultCode { get; set; }
        
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;
        
        [JsonPropertyName("payType")]
        public string PayType { get; set; } = string.Empty;
        
        [JsonPropertyName("responseTime")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public long ResponseTime { get; set; }
        
        [JsonPropertyName("extraData")]
        public string ExtraData { get; set; } = string.Empty;
        
        [JsonPropertyName("signature")]
        public string Signature { get; set; } = string.Empty;
    }
}
