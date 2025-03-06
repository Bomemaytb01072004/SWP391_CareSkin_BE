using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SWP391_CareSkin_BE.DTOS.Responses
{
    public class RegisterStaffDTO
    {

        [JsonPropertyName("UserName")]
        public string UserName { get; set; }

        [JsonPropertyName("Password")]
        public string Password { get; set; }

        [JsonPropertyName("ConfirmPassword")]
        public string ConfirmPassword { get; set; }

        [JsonPropertyName("Email")]
        public string? Email { get; set; }


        [JsonIgnore] public string? Phone { get; set; }
        [JsonIgnore] public string? FullName { get; set; }
        [JsonIgnore] public DateOnly? Dob { get; set; }
        [JsonIgnore] public string? ProfilePicture { get; set; }
    }
}
