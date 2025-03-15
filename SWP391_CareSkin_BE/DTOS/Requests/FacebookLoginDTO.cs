using System.ComponentModel.DataAnnotations;

namespace SWP391_CareSkin_BE.DTOs.Requests
{
    public class FacebookLoginDTO
    {
        [Required]
        public string Token { get; set; }
    }
}
