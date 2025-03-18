using System.Text.Json.Serialization;

namespace SWP391_CareSkin_BE.DTOS.Responses
{
    public class SocialLoginResponseDTO
    {
        public string token { get; set; }
        public SocialUserDTO user { get; set; }
    }

    public class SocialUserDTO
    {
        public int CustomerId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string PictureUrl { get; set; }
        public string role { get; set; }
    }
}
