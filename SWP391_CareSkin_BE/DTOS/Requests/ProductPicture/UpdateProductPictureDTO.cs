using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SWP391_CareSkin_BE.DTOS.ProductPicture
{
    public class UpdateProductPictureDTO
    {
        [Required]
        public IFormFile Image { get; set; }
    }
}
