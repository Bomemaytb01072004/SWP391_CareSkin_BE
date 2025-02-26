using System.ComponentModel.DataAnnotations;

namespace SWP391_CareSkin_BE.DTOS.Responses
{
    public class StaffResponseDTO
    {
        public int StaffId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public String Phone { get; set; }
        public DateOnly? DoB { get; set; }
        public string ProfilePicture { get; set; }
    }
}
