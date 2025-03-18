using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SWP391_CareSkin_BE.DTOS.Requests
{
    public class BrandImageUpdateRequestDTO
    {
        [Required(ErrorMessage = "Image file is required")]
        public IFormFile PictureFile { get; set; }
    }
}
