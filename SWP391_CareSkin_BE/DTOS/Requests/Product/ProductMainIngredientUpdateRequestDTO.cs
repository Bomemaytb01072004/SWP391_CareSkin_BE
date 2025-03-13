using System.ComponentModel.DataAnnotations;

namespace SWP391_CareSkin_BE.DTOs.Requests.Product
{
    public class ProductMainIngredientUpdateRequestDTO
    {
        public int ProductMainIngredientId { get; set; }
        
        [Required(ErrorMessage = "Ingredient name is required")]
        public string IngredientName { get; set; }
        
        public string Description { get; set; }
    }
}
