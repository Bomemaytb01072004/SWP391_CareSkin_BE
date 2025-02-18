using SWP391_CareSkin_BE.DTOS.Requests;
using SWP391_CareSkin_BE.DTOS.Responses;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Mappers
{
    public class ProductMapper
    {
        // Chuyển từ Product Entity sang ProductDTO
        public static ProductDTO ToDTO(Product product)
        {
            if (product == null)
                return null;

            return new ProductDTO
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Description = product.Description,
                BrandName = product.Brand?.Name,
                Variations = product.ProductVariations?.Select(v => new ProductVariationDTO
                {
                    ProductVariationId = v.ProductVariationId,
                    Ml = v.Ml,
                    Price = v.price
                }).ToList(),
                MainIngredients = product.ProductMainIngredients?.Select(m => new ProductMainIngredientDTO
                {
                    ProductMainIngredientId = m.ProductMainIngredientId,
                    IngredientName = m.IngredientName,
                    Description = m.Description
                }).ToList(),
                DetailIngredients = product.ProductDetailIngredients?.Select(d => new ProductDetailIngredientDTO
                {
                    ProductDetailIngredientId = d.ProductDetailIngredientId,
                    IngredientName = d.IngredientName
                }).ToList(),
                Usages = product.ProductUsages?.Select(u => new ProductUsageDTO
                {
                    ProductUsageId = u.ProductUsageId,
                    Step = u.Step,
                    Instruction = u.Instruction
                }).ToList()
            };
        }

        // Chuyển từ ProductCreateRequestDTO sang Product Entity
        public static Product ToEntity(ProductCreateRequestDTO request)
        {
            if (request == null)
                return null;

            return new Product
            {
                ProductName = request.ProductName,
                BrandId = request.BrandId,
                Description = request.Description,
                ProductVariations = request.Variations?.Select(v => new ProductVariation
                {
                    Ml = v.Ml,
                    price = v.Price
                }).ToList(),
                ProductMainIngredients = request.MainIngredients?.Select(m => new ProductMainIngredient
                {
                    IngredientName = m.IngredientName,
                    Description = m.Description
                }).ToList(),
                ProductDetailIngredients = request.DetailIngredients?.Select(m => new ProductDetailIngredient
                {
                    IngredientName = m.IngredientName,
                }).ToList(),
                ProductUsages = request.Usages?.Select(m => new ProductUsage
                {
                    Step = m.Step,
                    Instruction = m.Instruction
                }).ToList()
            };
        }

        // Cập nhật một Product Entity dựa trên ProductUpdateRequestDTO
        public static void UpdateEntity(Product product, ProductUpdateRequestDTO request)
        {
            if (product == null || request == null)
                return;

            product.ProductName = request.ProductName;
            product.BrandId = request.BrandId;
            product.Description = request.Description;

            // Ví dụ đơn giản: xoá toàn bộ Variation cũ và thêm Variation mới từ request.
            if (request.Variations != null)
            {
                product.ProductVariations.Clear();
                foreach (var variation in request.Variations)
                {
                    product.ProductVariations.Add(new ProductVariation
                    {
                        Ml = variation.Ml,
                        price = variation.Price
                    });
                }
            }
            if (request.MainIngredients != null)
            {
                product.ProductMainIngredients.Clear();
                foreach (var m in request.MainIngredients)
                {
                    product.ProductMainIngredients.Add(new ProductMainIngredient
                    {
                        IngredientName = m.IngredientName,
                        Description = m.Description
                    });
                }
            }
        }
    }
}
