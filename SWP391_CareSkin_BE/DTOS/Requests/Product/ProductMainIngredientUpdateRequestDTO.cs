using System.ComponentModel.DataAnnotations;

namespace SWP391_CareSkin_BE.DTOs.Requests.Product
{
    public class ProductMainIngredientUpdateRequestDTO
    {
        public int ProductMainIngredientId { get; set; }
        
        [Required(ErrorMessage = "Ingredient name is required")]
        [StringLength(100, ErrorMessage = "Ingredient name cannot exceed 100 characters")]
        public string IngredientName { get; set; }
        
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }
    }
}
