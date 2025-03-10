using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SWP391_CareSkin_BE.DTOS.Responses
{
    public class RegisterCustomerDTO
    {
        [JsonPropertyName("UserName")]
        public string UserName { get; set; }

        [JsonPropertyName("Password")]
        public string Password { get; set; }

        [JsonPropertyName("ConfirmPassword")]
        public string ConfirmPassword { get; set; }

        [JsonPropertyName("Email")]
        [Required]
        [MaxLength(100)]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@gmail\.com$", ErrorMessage = "Email phải có định dạng hợp lệ và kết thúc bằng @gmail.com")]
        public string Email { get; set; }


        //những thông tin không hiện trên register, user có thể cập nhật sau
        [JsonIgnore] public string? Phone { get; set; }
        [JsonIgnore] public string? FullName { get; set; }
        [JsonIgnore] public DateOnly? Dob { get; set; }
        [JsonIgnore] public string? Gender { get; set; }
        [JsonIgnore] public string? Address { get; set; }
        [JsonIgnore] public string? PictureUrl { get; set; }
    }
}
