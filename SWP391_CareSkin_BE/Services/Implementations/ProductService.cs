using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.DTOS.Requests;
using SWP391_CareSkin_BE.DTOS.Responses;
using SWP391_CareSkin_BE.Mappers;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using SWP391_CareSkin_BE.Services.Interfaces;
using SWP391_CareSkin_BE.Extensions;
using Microsoft.AspNetCore.Http;
using SWP391_CareSkin_BE.DTOS.ProductPicture;
using System;

namespace SWP391_CareSkin_BE.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IFirebaseService _firebaseService;
        private readonly IProductPictureService _productPictureService;
        private readonly IProductUsageRepository _productUsageRepository;
        private readonly IProductForSkinTypeRepository _productForSkinTypeRepository;
        private readonly IProductVariationRepository _productVariationRepository;
        private readonly IProductMainIngredientRepository _productMainIngredientRepository;
        private readonly IProductDetailIngredientRepository _productDetailIngredientRepository;
        private readonly ICartRepository _cartRepository;

        public ProductService(
            IProductRepository productRepository, 
            IFirebaseService firebaseService,
            IProductPictureService productPictureService,
            IProductUsageRepository productUsageRepository,
            IProductForSkinTypeRepository productForSkinTypeRepository,
            IProductVariationRepository productVariationRepository,
            IProductMainIngredientRepository productMainIngredientRepository,
            IProductDetailIngredientRepository productDetailIngredientRepository,
            ICartRepository cartRepository)
        {
            _productRepository = productRepository;
            _firebaseService = firebaseService;
            _productPictureService = productPictureService;
            _productUsageRepository = productUsageRepository;
            _productForSkinTypeRepository = productForSkinTypeRepository;
            _productVariationRepository = productVariationRepository;
            _productMainIngredientRepository = productMainIngredientRepository;
            _productDetailIngredientRepository = productDetailIngredientRepository;
            _cartRepository = cartRepository;
        }

        public async Task<List<ProductDTO>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllProductsAsync();
            return ProductMapper.ToDTOList(products);
        }

        public async Task<List<ProductDTO>> GetActiveProductsAsync()
        {
            var products = await _productRepository.GetActiveProductsAsync();
            return ProductMapper.ToDTOList(products);
        }

        public async Task<List<ProductDTO>> GetInactiveProductsAsync()
        {
            var products = await _productRepository.GetInactiveProductsAsync();
            return ProductMapper.ToDTOList(products);
        }

        public async Task<ProductDTO> GetProductByIdAsync(int productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            return ProductMapper.ToDTO(product);
        }

        public async Task<ProductDTO> CreateProductAsync(ProductCreateRequestDTO request, string pictureUrl)
        {
            // Kiểm tra xem sản phẩm với tên này đã tồn tại và đang active chưa
            var existingProduct = await _productRepository.GetProductByNameAsync(request.ProductName);
            
            if (existingProduct != null && existingProduct.IsActive)
            {
                // Throw ArgumentException with a specific message that will be caught by the controller
                throw new ArgumentException($"Sản phẩm với tên '{request.ProductName}' đã tồn tại");
            }

            var productEntity = ProductMapper.ToEntity(request, pictureUrl);
            await _productRepository.AddProductAsync(productEntity);
            var createdProduct = await _productRepository.GetProductByIdAsync(productEntity.ProductId);

            // Handle additional product pictures if provided
            if (request.AdditionalPictures != null && request.AdditionalPictures.Any())
            {
                foreach (var picture in request.AdditionalPictures)
                {
                    if (picture != null && picture.Length > 0)
                    {
                        var createPictureDto = new CreateProductPictureDTO
                        {
                            ProductId = createdProduct.ProductId,
                            Image = picture
                        };

                        await _productPictureService.CreateProductPictureAsync(createPictureDto);
                    }
                }

                // Reload the product to include the newly added pictures
                createdProduct = await _productRepository.GetProductByIdAsync(productEntity.ProductId);
            }

            return ProductMapper.ToDTO(createdProduct);
        }

        public async Task<ProductDTO> UpdateProductAsync(int productId, ProductUpdateRequestDTO request, string pictureUrl)
        {
            var existingProduct = await _productRepository.GetProductByIdAsync(productId);
            if (existingProduct == null)
                return null;

            // Cập nhật thông tin sản phẩm (trừ hình ảnh)
            if (request.ProductName != existingProduct.ProductName)
                existingProduct.ProductName = request.ProductName;

            if (request.BrandId != existingProduct.BrandId)
                existingProduct.BrandId = request.BrandId;

            if (request.Category != existingProduct.Category)
                existingProduct.Category = request.Category;

            if (request.Description != existingProduct.Description)
                existingProduct.Description = request.Description;

            // Xử lý hình ảnh sản phẩm
            if (!string.IsNullOrEmpty(pictureUrl))
            {
                if (!string.IsNullOrEmpty(existingProduct.PictureUrl))
                {
                    var oldFileName = ExtractFilenameFromFirebaseUrl(existingProduct.PictureUrl);
                    if (!string.IsNullOrEmpty(oldFileName))
                    {
                        await _firebaseService.DeleteImageAsync(oldFileName);
                    }
                }
                existingProduct.PictureUrl = pictureUrl;
            }

            // Cập nhật SkinTypes mà không xóa dữ liệu cũ
            if (request.ProductForSkinTypes != null && request.ProductForSkinTypes.Any())
            {
                foreach (var skinType in request.ProductForSkinTypes)
                {
                    var existingSkinType = existingProduct.ProductForSkinTypes
                        .FirstOrDefault(s => s.ProductForSkinTypeId == skinType.ProductForSkinTypeId); // Kiểm tra xem SkinType đã có chưa

                    if (existingSkinType != null)
                    {
                        // Nếu có SkinType trong cơ sở dữ liệu, cập nhật thông tin
                        existingSkinType.SkinTypeId = skinType.SkinTypeId;
                    }
                    else
                    {
                        // Nếu không có SkinType trong cơ sở dữ liệu, thêm mới
                        existingProduct.ProductForSkinTypes.Add(new ProductForSkinType
                        {
                            SkinTypeId = skinType.SkinTypeId
                        });
                    }

                }
            }

            // Cập nhật hoặc thêm mới ProductVariations mà không xóa dữ liệu cũ
            if (request.Variations != null && request.Variations.Any())
            {
                foreach (var variation in request.Variations)
                {
                    var existingVariation = existingProduct.ProductVariations
                        .FirstOrDefault(v => v.ProductVariationId == variation.ProductVariationId); // Kiểm tra nếu variation đã tồn tại

                    if (existingVariation != null)
                    {
                        // Cập nhật thông tin của variation đã có
                        existingVariation.Ml = variation.Ml;
                        existingVariation.Price = variation.Price;
                        existingVariation.SalePrice = variation.SalePrice ?? existingVariation.SalePrice; // Giữ giá cũ nếu không có giá sale mới
                    }
                    else
                    {
                        // Thêm variation mới nếu không tồn tại
                        existingProduct.ProductVariations.Add(new ProductVariation
                        {
                            Ml = variation.Ml,
                            Price = variation.Price,
                            SalePrice = variation.SalePrice ?? 0 // Khởi tạo SalePrice = 0 nếu không có giá
                        });
                    }
                }
            }

            // Cập nhật MainIngredients mà không xóa dữ liệu cũ
            if (request.MainIngredients != null && request.MainIngredients.Any())
            {
                foreach (var ingredient in request.MainIngredients)
                {
                    var existingIngredient = existingProduct.ProductMainIngredients
                        .FirstOrDefault(i => i.ProductMainIngredientId == ingredient.ProductMainIngredientId); // Kiểm tra sự tồn tại

                    if (existingIngredient != null)
                    {
                        // Cập nhật thông tin của ingredient đã có
                        existingIngredient.IngredientName = ingredient.IngredientName;
                        existingIngredient.Description = ingredient.Description;
                    }
                    else
                    {
                        // Thêm ingredient mới nếu không tồn tại
                        existingProduct.ProductMainIngredients.Add(new ProductMainIngredient
                        {
                            IngredientName = ingredient.IngredientName,
                            Description = ingredient.Description
                        });
                    }
                }
            }

            // Cập nhật DetailIngredients mà không xóa dữ liệu cũ
            if (request.DetailIngredients != null && request.DetailIngredients.Any())
            {
                foreach (var ingredient in request.DetailIngredients)
                {
                    var existingIngredient = existingProduct.ProductDetailIngredients
                        .FirstOrDefault(i => i.ProductDetailIngredientId == ingredient.ProductDetailIngredientId); // Kiểm tra sự tồn tại

                    if (existingIngredient != null)
                    {
                        // Cập nhật thông tin của ingredient đã có
                        existingIngredient.IngredientName = ingredient.IngredientName;
                    }
                    else
                    {
                        // Thêm ingredient mới nếu không tồn tại
                        existingProduct.ProductDetailIngredients.Add(new ProductDetailIngredient
                        {
                            IngredientName = ingredient.IngredientName
                        });
                    }
                }
            }

            // Cập nhật Usages mà không xóa dữ liệu cũ
            if (request.Usages != null && request.Usages.Any())
            {
                foreach (var usage in request.Usages)
                {
                    var existingUsage = existingProduct.ProductUsages
                        .FirstOrDefault(u => u.ProductUsageId == usage.ProductUsageId); // Kiểm tra sự tồn tại

                    if (existingUsage != null)
                    {
                        // Cập nhật thông tin usage nếu cần
                        existingUsage.Step = usage.Step;
                        existingUsage.Instruction = usage.Instruction;
                    }
                    else
                    {
                        // Thêm usage mới nếu không tồn tại
                        existingProduct.ProductUsages.Add(new ProductUsage
                        {
                            Step = usage.Step,
                            Instruction = usage.Instruction
                        });
                    }
                }
            }

            // Xử lý hình ảnh bổ sung của sản phẩm
            if (request.AdditionalPicturesToDelete != null && request.AdditionalPicturesToDelete.Any())
            {
                foreach (var pictureId in request.AdditionalPicturesToDelete)
                {
                    await _productPictureService.DeleteProductPictureAsync(pictureId);
                }
            }

            if (request.NewAdditionalPictures != null && request.NewAdditionalPictures.Any())
            {
                foreach (var picture in request.NewAdditionalPictures)
                {
                    if (picture != null && picture.Length > 0)
                    {
                        var createPictureDto = new CreateProductPictureDTO
                        {
                            ProductId = productId,
                            Image = picture
                        };

                        await _productPictureService.CreateProductPictureAsync(createPictureDto);
                    }
                }
            }

            // Cập nhật sản phẩm
            await _productRepository.UpdateProductAsync(existingProduct);
            var updatedProduct = await _productRepository.GetProductByIdAsync(productId);
            return ProductMapper.ToDTO(updatedProduct);
        }



        public async Task<(List<ProductDTO> Products, int TotalCount)> SearchProductsAsync(ProductSearchRequestDTO request)
        {
            var query = _productRepository.GetQueryable();

            // Only include active products
            query = query.Where(p => p.IsActive);

            query = query
                .ApplyKeywordFilter(request.Keyword)
                .ApplyCategoryFilter(request.Category)
                .ApplyBrandFilter(request.BrandId)
                .ApplyPriceFilter(request.MinPrice, request.MaxPrice)
                .ApplyMlFilter(request.MinMl, request.MaxMl)
                .ApplySorting(request.SortBy);

            var totalCount = await query.CountAsync();

            var pageNumber = request.PageNumber ?? 1;
            var pageSize = request.PageSize ?? 20;

            var products = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(p => p.Brand)
                .Include(p => p.ProductVariations)
                .Include(p => p.ProductMainIngredients)
                .Include(p => p.ProductDetailIngredients)
                .Include(p => p.ProductUsages)
                .Include(p => p.PromotionProducts.Where(pp => pp.Promotion.IsActive))
                    .ThenInclude(pp => pp.Promotion)
                .Include(p => p.ProductForSkinTypes)
                    .ThenInclude(ps => ps.SkinType)
                .Include(p => p.ProductPictures)
                .ToListAsync();

            return (products.Select(ProductMapper.ToDTO).ToList(), totalCount);
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            if (product == null)
                return false;

            // Implement soft delete by setting IsActive to false
            product.IsActive = false;
            await _productRepository.UpdateProductAsync(product);
            return true;
        }

        public async Task<bool> DeleteProductUsageAsync(int id)
        {
            var productUsage = await _productUsageRepository.GetByIdAsync(id);
            if (productUsage == null) return false;

            await _productUsageRepository.DeleteAsync(productUsage);
            return true;
        }

        public async Task<bool> DeleteProductForSkinTypeAsync(int id)
        {
            var productForSkinType = await _productForSkinTypeRepository.GetByIdAsync(id);
            if (productForSkinType == null) return false;

            await _productForSkinTypeRepository.DeleteAsync(productForSkinType);
            return true;
        }

        public async Task<bool> DeleteProductVariationAsync(int id)
        {
            var productVariation = await _productVariationRepository.GetByIdAsync(id);
            if (productVariation == null) return false;

            // First, delete any cart items that reference this product variation
            await _cartRepository.RemoveCartItemsByProductVariationIdAsync(id);

            await _productVariationRepository.DeleteAsync(productVariation);
            return true;
        }

        public async Task<bool> DeleteProductMainIngredientAsync(int id)
        {
            var productMainIngredient = await _productMainIngredientRepository.GetByIdAsync(id);
            if (productMainIngredient == null) return false;

            await _productMainIngredientRepository.DeleteAsync(productMainIngredient);
            return true;
        }

        public async Task<bool> DeleteProductDetailIngredientAsync(int id)
        {
            var productDetailIngredient = await _productDetailIngredientRepository.GetByIdAsync(id);
            if (productDetailIngredient == null) return false;

            await _productDetailIngredientRepository.DeleteAsync(productDetailIngredient);
            return true;
        }

        // Helper method to extract filename from Firebase Storage URL
        private string ExtractFilenameFromFirebaseUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return null;

            try
            {
                var uri = new Uri(url);
                var path = Uri.UnescapeDataString(uri.AbsolutePath);
                return path.Split(new[] { "/o/" }, StringSplitOptions.None)[1];
            }
            catch
            {
                // If URL parsing fails, try a simpler approach
                return url.Split('/').Last().Split('?').First();
            }
        }
    }
}
