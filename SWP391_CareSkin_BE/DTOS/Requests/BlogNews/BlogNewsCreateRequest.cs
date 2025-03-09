using System.ComponentModel.DataAnnotations;

namespace SWP391_CareSkin_BE.DTOs.Requests.BlogNews
{
    public class BlogNewsCreateRequest
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public IFormFile? PictureFile { get; set; }

        [Required] // Thêm Required nếu CustomerId bắt buộc
        public int CustomerId { get; set; }
    }
}
