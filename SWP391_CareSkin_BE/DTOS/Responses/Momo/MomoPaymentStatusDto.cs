using System;
using System.Text.Json.Serialization;

namespace SWP391_CareSkin_BE.DTOS.Responses.Momo
{
    /// <summary>
    /// DTO for Momo payment status
    /// </summary>
    public class MomoPaymentStatusDto
    {
        [JsonPropertyName("orderId")]
        public int OrderId { get; set; }
        
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }
        
        [JsonPropertyName("isPaid")]
        public bool IsPaid { get; set; }
        
        [JsonPropertyName("paymentTime")]
        public DateTime? PaymentTime { get; set; }
    }
}
