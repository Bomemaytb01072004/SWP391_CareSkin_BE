using System.ComponentModel.DataAnnotations;

namespace SWP391_CareSkin_BE.DTOs.Requests.Product
{
    public class ProductDetailIngredientUpdateRequestDTO
    {
        public int ProductDetailIngredientId { get; set; }
        
        [Required(ErrorMessage = "Ingredient name is required")]
        [StringLength(100, ErrorMessage = "Ingredient name cannot exceed 100 characters")]
        public string IngredientName { get; set; }
    }
}
