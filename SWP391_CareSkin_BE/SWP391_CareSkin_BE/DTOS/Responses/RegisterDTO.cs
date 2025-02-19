using System.Text.Json.Serialization;

namespace SWP391_CareSkin_BE.DTOS.Responses
{
    public class RegisterDTO
    {
        [JsonPropertyName("UserName")]
        public string UserName { get; set; }

        [JsonPropertyName("Password")]
        public string Password { get; set; }

        [JsonPropertyName("ConfirmPassword")]
        public string ConfirmPassword { get; set; }

        [JsonPropertyName("Email")]
        public string Email { get; set; }

        
        //những thông tin không hiện trên register, user có thể cập nhật sau
        [JsonIgnore] public string? FullName { get; set; }
        [JsonIgnore] public DateTime? Dob { get; set; }
        [JsonIgnore] public string? Gender { get; set; }
        [JsonIgnore] public string? Address { get; set; }
        [JsonIgnore] public string? ProfilePicture { get; set; }
    }
}
