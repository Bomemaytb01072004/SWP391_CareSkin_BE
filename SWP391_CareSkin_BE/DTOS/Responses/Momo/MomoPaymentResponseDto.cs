using System.Text.Json.Serialization;

namespace SWP391_CareSkin_BE.DTOS.Responses.Momo
{
    /// <summary>
    /// DTO for Momo payment response
    /// </summary>
    public class MomoPaymentResponseDto
    {
        [JsonPropertyName("partnerCode")]
        public string? PartnerCode { get; set; }

        [JsonPropertyName("orderId")]
        public string? OrderId { get; set; }

        [JsonPropertyName("requestId")]
        public string? RequestId { get; set; }

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("responseTime")]
        public long ResponseTime { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("resultCode")]
        public int ErrorCode { get; set; }

        [JsonPropertyName("payUrl")]
        public string? PayUrl { get; set; }

        [JsonPropertyName("deeplink")]
        public string? Deeplink { get; set; }

        [JsonPropertyName("qrCodeUrl")]
        public string? QrCodeUrl { get; set; }
    }
}
