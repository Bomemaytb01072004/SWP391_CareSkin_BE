namespace SWP391_CareSkin_BE.DTOS.Requests
{
    public class UpdateProfileStaffDTO
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateOnly? Dob { get; set; }
        public IFormFile PictureFile { get; set; }
    }
}
