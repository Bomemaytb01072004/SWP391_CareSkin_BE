using System.ComponentModel.DataAnnotations;

namespace SWP391_CareSkin_BE.DTOs.Requests.Product
{
    public class ProductVariationUpdateRequestDTO
    {
        public int? ProductVariationId { get; set; }
        
        [Required(ErrorMessage = "Ml value is required")]
        public int Ml { get; set; }
        
        [Required(ErrorMessage = "Price is required")]
        public decimal Price { get; set; }
        
        public decimal? SalePrice { get; set; }
    }
}
