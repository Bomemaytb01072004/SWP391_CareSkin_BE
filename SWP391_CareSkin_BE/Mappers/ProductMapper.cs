using SWP391_CareSkin_BE.DTOs.Responses.Product;
using SWP391_CareSkin_BE.DTOs.Requests.Product;
using SWP391_CareSkin_BE.DTOS.Requests;
using SWP391_CareSkin_BE.DTOS.Responses;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.DTOS.ProductPicture;
using SWP391_CareSkin_BE.DTOS.Responses.Routine;

namespace SWP391_CareSkin_BE.Mappers
{
    public class ProductMapper
    {
        // Convert from Product Entity to ProductDTO
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
                PictureUrl = product.PictureUrl,
                AverageRating = product.AverageRating,
                IsActive = product.IsActive,

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
                }).ToList(),
                ProductPictures = product.ProductPictures?.Select(pp => new ProductPictureDTO
                {
                    ProductPictureId = pp.ProductPictureId,
                    ProductId = pp.ProductId,
                    ProductName = product.ProductName,
                    PictureUrl = pp.PictureUrl
                }).ToList()
            };
        }

        // Convert a list of Product entities to a list of ProductDTOs
        public static List<ProductDTO> ToDTOList(IEnumerable<Product> products)
        {
            if (products == null)
                return new List<ProductDTO>();

            return products.Select(ToDTO).ToList();
        }

        // Convert from Product Entity to ProductForRoutineDTO
        public static ProductForRoutineDTO ToProductForRoutineDTO(Product product)
        {
            if (product == null)
                return null;

            return new ProductForRoutineDTO
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Description = product.Description,
                Category = product.Category,
                BrandName = product.Brand?.Name,
                PictureUrl = product.PictureUrl,
                AverageRating = product.AverageRating,
                Variations = product.ProductVariations?.Select(v => new ProductVariationDTO
                {
                    ProductVariationId = v.ProductVariationId,
                    Ml = v.Ml,
                    Price = v.Price,
                    SalePrice = v.SalePrice
                }).ToList()
            };
        }

        // Convert a list of Product entities to a list of ProductForRoutineDTOs
        public static List<ProductForRoutineDTO> ToProductForRoutineDTOList(IEnumerable<Product> products)
        {
            if (products == null)
                return new List<ProductForRoutineDTO>();

            return products.Select(ToProductForRoutineDTO).ToList();
        }

        // Convert from Product Entity to simplified ProductDTO for routine responses
        public static ProductDTO ToSimplifiedDTO(Product product)
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
                PictureUrl = product.PictureUrl,
                AverageRating = product.AverageRating,
                IsActive = product.IsActive,
                Variations = product.ProductVariations?.Select(v => new ProductVariationDTO
                {
                    ProductVariationId = v.ProductVariationId,
                    Ml = v.Ml,
                    Price = v.Price,
                    SalePrice = v.SalePrice
                }).ToList(),
                // Initialize other properties as empty lists to avoid null reference exceptions
                PromotionProducts = new List<PromotionProductDTO>(),
                ProductForSkinTypes = new List<ProductForSkinTypeDTO>(),
                MainIngredients = new List<ProductMainIngredientDTO>(),
                DetailIngredients = new List<ProductDetailIngredientDTO>(),
                Usages = new List<ProductUsageDTO>(),
                ProductPictures = new List<ProductPictureDTO>()
            };
        }

        // Convert a list of Product entities to a list of simplified ProductDTOs for routine responses
        public static List<ProductDTO> ToSimplifiedDTOList(IEnumerable<Product> products)
        {
            if (products == null)
                return new List<ProductDTO>();

            return products.Select(ToSimplifiedDTO).ToList();
        }

        // Convert from ProductCreateRequestDTO to Product Entity
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
                PictureUrl = pictureUrl,
                AverageRating = 0, // Initialize average rating to 0 for new products
                IsActive = true, // Set IsActive to true when creating a new product

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

        // Update a Product Entity based on ProductUpdateRequestDTO
        public static void UpdateEntity(Product product, ProductUpdateRequestDTO request, string pictureUrl = null)
        {
            if (product == null || request == null)
                return;

            product.ProductName = request.ProductName;
            product.BrandId = request.BrandId;
            product.Category = request.Category;
            product.Description = request.Description;

            // Set new picture if provided
            if (!string.IsNullOrEmpty(pictureUrl))
            {
                product.PictureUrl = pictureUrl;
            }

            // Update ProductForSkinTypes
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

            // Update ProductVariations
            if (request.Variations != null)
            {
                product.ProductVariations.Clear();
                foreach (var variation in request.Variations)
                {
                    product.ProductVariations.Add(new ProductVariation
                    {
                        ProductVariationId = variation.ProductVariationId ?? 0,
                        Ml = variation.Ml,
                        Price = variation.Price,
                        SalePrice = variation.SalePrice ?? 0 // Use provided SalePrice or default to 0
                    });
                }
            }

            // Update ProductMainIngredients
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

            // Update ProductDetailIngredients
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

            // Update ProductUsages
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
