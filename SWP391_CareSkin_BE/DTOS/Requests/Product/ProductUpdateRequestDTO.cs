using SWP391_CareSkin_BE.DTOs.Requests.Product;
using SWP391_CareSkin_BE.DTOS.Responses;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        public List<ProductForSkinTypeUpdateRequestDTO>? ProductForSkinTypes { get; set; }
        public List<ProductVariationUpdateRequestDTO>? Variations { get; set; }
        public List<ProductMainIngredientUpdateRequestDTO>? MainIngredients { get; set; }
        public List<ProductDetailIngredientUpdateRequestDTO>? DetailIngredients { get; set; }
        public List<ProductUsageUpdateRequestDTO>? Usages { get; set; }
    }
}
