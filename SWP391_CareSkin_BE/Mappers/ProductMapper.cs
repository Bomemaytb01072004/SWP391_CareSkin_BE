using SWP391_CareSkin_BE.DTOs.Responses.Product;
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
                Category = product.Category,
                BrandName = product.Brand?.Name,

                // Trả về URL ảnh
                PictureUrl = product.PictureUrl,


                PromotionProducts = product.PromotionProducts?
                    .Where(p => p.IsActive) // Only include active promotions
                    .Select(p => new PromotionProductDTO
                    {
                        PromotionProductId = p.PromotionProductId,
                        PromotionId = p.PromotionId,
                        ProductId = p.ProductId,
                        DiscountPercent = p.Promotion.DiscountPercent,
                        Start_Date = p.Promotion.Start_Date,
                        End_Date = p.Promotion.End_Date,
                        IsActive = p.IsActive
                    }).ToList(),
                ProductForSkinTypes = product.ProductForSkinTypes?.Select(s => new ProductForSkinTypeDTO
                {
                    ProductForSkinTypeId = s.ProductForSkinTypeId,
                    ProductId = s.ProductId,
                    SkinTypeId = s.SkinTypeId,
                    TypeName = s.SkinType.TypeName
                }).ToList(),
                Variations = product.ProductVariations?.Select(v => new ProductVariationDTO
                {
                    ProductVariationId = v.ProductVariationId,
                    Ml = v.Ml,
                    Price = v.Price,
                    SalePrice = v.SalePrice
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
        // Có thể truyền vào tham số pictureUrl (sau khi upload xong) nếu muốn set luôn
        public static Product ToEntity(ProductCreateRequestDTO request, string pictureUrl = null)
        {
            if (request == null)
                return null;

            var product = new Product
            {
                ProductName = request.ProductName,
                BrandId = request.BrandId,
                Category = request.Category,
                Description = request.Description,

                // Sau khi upload ảnh, bạn có thể gán pictureUrl vào đây
                PictureUrl = pictureUrl,

                ProductForSkinTypes = request.ProductForSkinTypes?.Select(s => new ProductForSkinType
                {
                    SkinTypeId = s.SkinTypeId
                }).ToList(),
                ProductVariations = request.Variations?.Select(v => new ProductVariation
                {
                    Ml = v.Ml,
                    Price = v.Price,
                    SalePrice = 0 // Initialize SalePrice to 0 for new variations
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

            return product;
        }

        // Cập nhật một Product Entity dựa trên ProductUpdateRequestDTO
        public static void UpdateEntity(Product product, ProductUpdateRequestDTO request, string pictureUrl = null)
        {
            if (product == null || request == null)
                return;

            product.ProductName = request.ProductName;
            product.BrandId = request.BrandId;
            product.Category = request.Category;
            product.Description = request.Description;

            // Nếu có ảnh mới, set lại
            if (!string.IsNullOrEmpty(pictureUrl))
            {
                product.PictureUrl = pictureUrl;
            }

            // Cập nhật ProductForSkinTypes
            if (request.ProductForSkinTypes != null)
            {
                product.ProductForSkinTypes.Clear();
                foreach (var skinType in request.ProductForSkinTypes)
                {
                    product.ProductForSkinTypes.Add(new ProductForSkinType
                    {
                        SkinTypeId = skinType.SkinTypeId
                    });
                }
            }

            // Cập nhật ProductVariations
            if (request.Variations != null)
            {
                product.ProductVariations.Clear();
                foreach (var variation in request.Variations)
                {
                    product.ProductVariations.Add(new ProductVariation
                    {
                        Ml = variation.Ml,
                        Price = variation.Price,
                        SalePrice = 0 // Initialize SalePrice to 0 for new variations
                    });
                }
            }

            // Cập nhật ProductMainIngredients
            if (request.MainIngredients != null)
            {
                product.ProductMainIngredients.Clear();
                foreach (var ingredient in request.MainIngredients)
                {
                    product.ProductMainIngredients.Add(new ProductMainIngredient
                    {
                        IngredientName = ingredient.IngredientName,
                        Description = ingredient.Description
                    });
                }
            }

            // Cập nhật ProductDetailIngredients
            if (request.DetailIngredients != null)
            {
                product.ProductDetailIngredients.Clear();
                foreach (var ingredient in request.DetailIngredients)
                {
                    product.ProductDetailIngredients.Add(new ProductDetailIngredient
                    {
                        IngredientName = ingredient.IngredientName
                    });
                }
            }

            // Cập nhật ProductUsages
            if (request.Usages != null)
            {
                product.ProductUsages.Clear();
                foreach (var usage in request.Usages)
                {
                    product.ProductUsages.Add(new ProductUsage
                    {
                        Step = usage.Step,
                        Instruction = usage.Instruction
                    });
                }
            }
        }
    }
}
