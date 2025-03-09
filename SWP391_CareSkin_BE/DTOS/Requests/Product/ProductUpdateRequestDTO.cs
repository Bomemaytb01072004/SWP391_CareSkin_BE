using SWP391_CareSkin_BE.DTOs.Requests.Product;
using SWP391_CareSkin_BE.DTOS.Responses;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace SWP391_CareSkin_BE.DTOS.Requests
{
    public class ProductUpdateRequestDTO
    {
        public string ProductName { get; set; }
        public int BrandId { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }

        // File ảnh chính gửi từ client
        public IFormFile? PictureFile { get; set; }
        
        // Danh sách ID của các ảnh phụ cần xóa
        public List<int>? AdditionalPicturesToDelete { get; set; }
        
        // Danh sách các ảnh phụ mới gửi từ client
        public List<IFormFile>? NewAdditionalPictures { get; set; }

        public List<ProductForSkinTypeCreateRequestDTO> ProductForSkinTypes { get; set; }
        public List<ProductVariationCreateRequestDTO> Variations { get; set; }
        public List<ProductMainIngredientCreateRequestDTO> MainIngredients { get; set; }
        public List<ProductDetailIngredientCreateRequestDTO> DetailIngredients { get; set; }
        public List<ProductUsageCreateRequestDTO> Usages { get; set; }
    }
}
