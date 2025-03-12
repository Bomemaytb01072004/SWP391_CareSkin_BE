using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SWP391_CareSkin_BE.DTOS.ProductPicture
{
    public class CreateProductPictureDTO
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public IFormFile Image { get; set; }
    }
}
