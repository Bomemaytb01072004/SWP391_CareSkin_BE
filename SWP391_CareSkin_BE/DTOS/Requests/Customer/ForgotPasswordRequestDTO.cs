using System.ComponentModel.DataAnnotations;

namespace SWP391_CareSkin_BE.DTOs.Requests.Customer
{
    public class ForgotPasswordRequestDTO
    {
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
