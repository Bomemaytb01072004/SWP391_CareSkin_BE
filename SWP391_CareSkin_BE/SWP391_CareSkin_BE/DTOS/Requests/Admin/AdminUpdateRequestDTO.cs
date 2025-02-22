using System.ComponentModel.DataAnnotations;

namespace SWP391_CareSkin_BE.DTOs.Requests.Admin
{
    public class AdminUpdateRequestDTO
    {
        [Required]
        public string Password { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public int Phone { get; set; }

        public DateOnly DoB { get; set; }

        public string ProfilePicture { get; set; }
    }
}
