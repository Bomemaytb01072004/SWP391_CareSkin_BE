using System.ComponentModel.DataAnnotations;

namespace SWP391_CareSkin_BE.DTOS.Responses
{
    public class RegisterDTO
    {

        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
    }
}
