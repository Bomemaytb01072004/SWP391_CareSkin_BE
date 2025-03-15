using System.ComponentModel.DataAnnotations;

namespace SWP391_CareSkin_BE.DTOs.Requests.BlogNews
{
    public class BlogNewsCreateRequest
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public IFormFile? PictureFile { get; set; }

    }
}
