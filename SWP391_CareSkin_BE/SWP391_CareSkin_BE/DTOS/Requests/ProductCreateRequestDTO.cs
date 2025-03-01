﻿using SWP391_CareSkin_BE.DTOS.Responses;

namespace SWP391_CareSkin_BE.DTOS.Requests
{
    public class ProductCreateRequestDTO
    {
        public string ProductName { get; set; }
        public int BrandId { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }

        // Dữ liệu cho variation khi tạo sản phẩm
        public List<ProductVariationCreateRequestDTO> Variations { get; set; }

        public List<ProductMainIngredientCreateRequestDTO> MainIngredients { get; set; }

        public List<ProductDetailIngredientCreateRequestDTO> DetailIngredients { get; set; }

        public List<ProductUsageCreateRequestDTO> Usages { get; set; }
    }
}
