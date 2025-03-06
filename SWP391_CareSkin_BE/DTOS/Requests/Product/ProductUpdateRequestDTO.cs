using SWP391_CareSkin_BE.DTOs.Requests.Product;
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

        public List<ProductForSkinTypeCreateRequestDTO> ProductForSkinTypes { get; set; }
        public List<ProductVariationCreateRequestDTO> Variations { get; set; }
        public List<ProductMainIngredientCreateRequestDTO> MainIngredients { get; set; }
        public List<ProductDetailIngredientCreateRequestDTO> DetailIngredients { get; set; }
        public List<ProductUsageCreateRequestDTO> Usages { get; set; }
    }
}
