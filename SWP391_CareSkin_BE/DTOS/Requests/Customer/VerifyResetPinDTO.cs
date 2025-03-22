using System.ComponentModel.DataAnnotations;

namespace SWP391_CareSkin_BE.DTOs.Requests.Customer
{
    public class VerifyResetPinDTO
    {
        [Required, MaxLength(6)]
        public string ResetPin { get; set; }
    }
}
