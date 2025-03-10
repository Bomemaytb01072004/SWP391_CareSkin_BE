using System.ComponentModel.DataAnnotations;

namespace SWP391_CareSkin_BE.DTOs.Requests
{
    public class UpdateProfileCustomerDTO
    {
        public string? FullName { get; set; }
        [Required]
        [MaxLength(100)]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@gmail\.com$", ErrorMessage = "Email phải có định dạng hợp lệ và kết thúc bằng @gmail.com")]
        public string Email { get; set; }

        [RegularExpression(@"^0\d{10}$", ErrorMessage = "Số điện thoại phải bắt đầu bằng 0 và có 11 chữ số.")]
        public string? Phone { get; set; }
        public DateOnly? Dob { get; set; }
        public string? Gender { get; set; }
        public IFormFile PictureFile { get; set; }
        public string? Address { get; set; }
    }
}
