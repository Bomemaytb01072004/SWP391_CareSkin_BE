namespace SWP391_CareSkin_BE.DTOs.Requests
{
    public class UpdateProfileCustomerDTO
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateOnly? Dob { get; set; }
        public string? Gender { get; set; }
        public IFormFile PictureFile { get; set; }
        public string? Address { get; set; }
    }
}
