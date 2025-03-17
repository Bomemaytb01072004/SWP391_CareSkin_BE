using System.Text.Json.Serialization;

namespace SWP391_CareSkin_BE.DTOS.Requests.Momo
{
    /// <summary>
    /// DTO for Momo callback data
    /// </summary>
    public class MomoCallbackDto
    {
        [JsonPropertyName("requestId")]
        public string RequestId { get; set; } = string.Empty;
        
        [JsonPropertyName("orderId")]
        public string OrderId { get; set; } = string.Empty;
        
        [JsonPropertyName("amount")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public decimal Amount { get; set; }
        
        [JsonPropertyName("resultCode")]
        public int ResultCode { get; set; }
        
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;
        
        [JsonPropertyName("signature")]
        public string Signature { get; set; } = string.Empty;
        
        [JsonPropertyName("extraData")]
        public string ExtraData { get; set; } = string.Empty;
        
        [JsonPropertyName("payType")]
        public string PayType { get; set; } = string.Empty;
        
        [JsonPropertyName("transId")]
        public string TransId { get; set; } = string.Empty;
    }
}
