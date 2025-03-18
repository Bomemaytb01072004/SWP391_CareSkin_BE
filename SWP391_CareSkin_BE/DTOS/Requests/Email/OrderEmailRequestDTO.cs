using System.ComponentModel.DataAnnotations;

namespace SWP391_CareSkin_BE.DTOs.Requests.Email
{
    public class OrderEmailRequestDTO
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string CustomerName { get; set; }
    }
}
