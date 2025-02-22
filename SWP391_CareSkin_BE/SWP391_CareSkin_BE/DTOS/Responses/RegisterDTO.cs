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

        [JsonPropertyName("dob")]
        public DateOnly Dob {  get; set; }

        [JsonPropertyName("gender")]
        public string Gender { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("profilePicture")]
        public string ProfilePicture { get; set; }

       
    }
}
