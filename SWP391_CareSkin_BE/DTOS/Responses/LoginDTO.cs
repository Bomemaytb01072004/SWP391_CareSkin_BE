using System.Text.Json.Serialization;

namespace SWP391_CareSkin_BE.DTOS
{
    public class LoginDTO

    {
        [JsonPropertyName("UserName")]
        public string UserName { get; set; }

        [JsonPropertyName("Password")]
        public string Password { get; set; }
    }
}
