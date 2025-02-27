using System.Text.Json.Serialization;

namespace SWP391_CareSkin_BE.DTOS.Requests
{
    public class DeleteStaffDTO
    {
        [JsonPropertyName("Password")]
        public string Password { get; set; }
    }
}
