using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SWP391_CareSkin_BE.DTOS.Responses
{
    public class StaffDTO
    {
        public int StaffId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public String Phone { get; set; }
        public DateOnly? DoB { get; set; }
        public string PictureUrl { get; set; }
        public bool IsActive { get; set; }
        public string? token { get; set; }
        public string Role { get; set; }
    }
}
