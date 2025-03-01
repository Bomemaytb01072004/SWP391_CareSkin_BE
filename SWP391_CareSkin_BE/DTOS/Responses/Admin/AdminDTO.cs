using System.ComponentModel.DataAnnotations;

namespace SWP391_CareSkin_BE.DTOs.Responses
{
    public class AdminDTO
    {
        public int AdminId { get; set; }
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public DateOnly? DoB { get; set; }

        public string PictureUrl { get; set; }
    }
}
