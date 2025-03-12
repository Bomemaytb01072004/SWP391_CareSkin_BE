using System.ComponentModel.DataAnnotations;

namespace SWP391_CareSkin_BE.DTOs.Requests.Product
{
    public class ProductVariationUpdateRequestDTO
    {
        public int? ProductVariationId { get; set; }
        
        [Required(ErrorMessage = "Ml value is required")]
        [Range(1, 10000, ErrorMessage = "Ml must be between 1 and 10000")]
        public int Ml { get; set; }
        
        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 100000000, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }
        
        public decimal? SalePrice { get; set; }
    }
}
