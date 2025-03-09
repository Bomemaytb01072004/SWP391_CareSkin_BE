using SWP391_CareSkin_BE.DTOs.Responses.Product;
using SWP391_CareSkin_BE.DTOS.ProductPicture;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.DTOS.Responses
{
    public class ProductDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string BrandName { get; set; }
        public string PictureUrl { get; set; }
        public double AverageRating { get; set; }

        public List<ProductForSkinTypeDTO> ProductForSkinTypes { get; set; }

        // Danh sách các Variation của sản phẩm
        public List<ProductVariationDTO> Variations { get; set; }

        public List<PromotionProductDTO> PromotionProducts { get; set; }

        // Danh sách các thành phần chính
        public List<ProductMainIngredientDTO> MainIngredients { get; set; }

        // Danh sách các thành phần phụ
        public List<ProductDetailIngredientDTO> DetailIngredients { get; set; }

        // Danh sách cách sử dụng sản phẩm
        public List<ProductUsageDTO> Usages { get; set; }
        
        // Danh sách các ảnh phụ của sản phẩm
        public List<ProductPictureDTO> ProductPictures { get; set; }
    }
}
