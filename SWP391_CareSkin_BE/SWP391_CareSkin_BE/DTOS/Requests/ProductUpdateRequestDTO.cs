using SWP391_CareSkin_BE.DTOS.Responses;

namespace SWP391_CareSkin_BE.DTOS.Requests
{
    public class ProductUpdateRequestDTO
    {
        public string ProductName { get; set; }
        public int BrandId { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }

        public IFormFile PictureFile { get; set; }
        public string PictureUrl { get; set; } // để lưu URL ảnh sau upload

        public List<ProductVariationCreateRequestDTO> Variations { get; set; }

        public List<ProductMainIngredientDTO> MainIngredients { get; set; }
    }
}
