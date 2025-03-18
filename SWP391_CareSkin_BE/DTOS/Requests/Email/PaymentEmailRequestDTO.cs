using System.ComponentModel.DataAnnotations;

namespace SWP391_CareSkin_BE.DTOs.Requests.Email
{
    public class PaymentEmailRequestDTO
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string CustomerName { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Payment amount must be greater than 0")]
        public decimal PaymentAmount { get; set; }

        [Required]
        public string PaymentMethod { get; set; }
    }
}
