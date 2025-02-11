using System.ComponentModel.DataAnnotations;

namespace SWP391_CareSkin_BE.Models
{
    public class RegisterRequest
    {
     
        public String UserName {get; set;}
        public String Password {get; set;}
        public String ConfirmPassword { get; set; }
        public string Email {get; set;}
    }
}
