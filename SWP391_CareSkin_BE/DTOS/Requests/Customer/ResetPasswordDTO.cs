using System.ComponentModel.DataAnnotations;

namespace SWP391_CareSkin_BE.DTOs.Requests.Customer
{
    public class ResetPasswordDTO
    {
        [Required]
        public string Email { get; set; }

        [Required, MinLength(6)]
        public string NewPassword { get; set; }
    }
}
