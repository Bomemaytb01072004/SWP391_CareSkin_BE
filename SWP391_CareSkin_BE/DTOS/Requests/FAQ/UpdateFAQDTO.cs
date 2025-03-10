using System.ComponentModel;

namespace SWP391_CareSkin_BE.DTOs.Requests.FAQ
{
    public class UpdateFAQDTO
    {
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
    }
}
