using System.ComponentModel.DataAnnotations;

namespace SWP391_CareSkin_BE.DTOs.Requests.Email
{
    public class CustomEmailRequestDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 1)]
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }
    }
}
