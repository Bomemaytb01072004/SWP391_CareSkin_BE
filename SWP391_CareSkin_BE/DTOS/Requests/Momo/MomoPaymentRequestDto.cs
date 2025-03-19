using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SWP391_CareSkin_BE.DTOS.Requests.Momo
{
    /// <summary>
    /// DTO for requesting a Momo payment
    /// </summary>
    public class MomoPaymentRequestDto
    {
        [Required]
        [JsonPropertyName("OrderId")]
        public int OrderId { get; set; }
        
        [Required]
        [JsonPropertyName("Amount")]
        [Range(1000, 50000000, ErrorMessage = "Số tiền phải từ 1,000 VND đến 50,000,000 VND")]
        public decimal Amount { get; set; }
    }
}
