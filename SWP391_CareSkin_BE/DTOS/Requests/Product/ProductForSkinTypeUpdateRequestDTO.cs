using System.ComponentModel.DataAnnotations;

namespace SWP391_CareSkin_BE.DTOs.Requests.Product
{
    public class ProductForSkinTypeUpdateRequestDTO
    {
        public int ProductForSkinTypeId { get; set; }
        
        [Required(ErrorMessage = "Product ID is required")]
        public int ProductId { get; set; }
        
        [Required(ErrorMessage = "Skin type ID is required")]
        public int SkinTypeId { get; set; }
    }
}
