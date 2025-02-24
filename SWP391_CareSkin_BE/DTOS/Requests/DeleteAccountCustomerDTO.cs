using System.Text.Json.Serialization;

namespace SWP391_CareSkin_BE.DTOs.Requests
{
    public class DeleteAccountCustomerDTO
    {
        [JsonPropertyName("Password")]
        public string Password { get; set; }
    }
}
