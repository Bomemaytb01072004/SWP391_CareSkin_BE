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

        public ProductService(
            IProductRepository productRepository, 
            IFirebaseService firebaseService,
            IProductPictureService productPictureService)
        {
            _productRepository = productRepository;
            _firebaseService = firebaseService;
            _productPictureService = productPictureService;
        }

        public async Task<List<ProductDTO>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllProductsAsync();
            return ProductMapper.ToDTOList(products);
        }

        public async Task<ProductDTO> GetProductByIdAsync(int productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            return ProductMapper.ToDTO(product);
        }

        public async Task<ProductDTO> CreateProductAsync(ProductCreateRequestDTO request, string pictureUrl)
        {
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

            if (request.ProductName != existingProduct.ProductName)
                existingProduct.ProductName = request.ProductName;

            if (request.BrandId != existingProduct.BrandId)
                existingProduct.BrandId = request.BrandId;

            if (request.Category != existingProduct.Category)
                existingProduct.Category = request.Category;

            if (request.Description != existingProduct.Description)
                existingProduct.Description = request.Description;

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

            // Update SkinTypes if provided
            if (request.ProductForSkinTypes != null)
            {
                existingProduct.ProductForSkinTypes.Clear();
                foreach (var skinType in request.ProductForSkinTypes)
                {
                    existingProduct.ProductForSkinTypes.Add(new ProductForSkinType
                    {
                        SkinTypeId = skinType.SkinTypeId
                    });
                }
            }

            if (request.Variations != null && request.Variations.Any())
            {
                existingProduct.ProductVariations.Clear();
                foreach (var variation in request.Variations)
                {
                    existingProduct.ProductVariations.Add(new ProductVariation
                    {
                        Ml = variation.Ml,
                        Price = variation.Price,
                        SalePrice = 0 // Initialize SalePrice to 0 for new variations
                    });
                }
            }

            if (request.MainIngredients != null && request.MainIngredients.Any())
            {
                existingProduct.ProductMainIngredients.Clear();
                foreach (var ingredient in request.MainIngredients)
                {
                    existingProduct.ProductMainIngredients.Add(new ProductMainIngredient
                    {
                        IngredientName = ingredient.IngredientName,
                        Description = ingredient.Description
                    });
                }
            }

            if (request.DetailIngredients != null && request.DetailIngredients.Any())
            {
                existingProduct.ProductDetailIngredients.Clear();
                foreach (var ingredient in request.DetailIngredients)
                {
                    existingProduct.ProductDetailIngredients.Add(new ProductDetailIngredient
                    {
                        IngredientName = ingredient.IngredientName
                    });
                }
            }

            if (request.Usages != null && request.Usages.Any())
            {
                existingProduct.ProductUsages.Clear();
                foreach (var usage in request.Usages)
                {
                    existingProduct.ProductUsages.Add(new ProductUsage
                    {
                        Step = usage.Step,
                        Instruction = usage.Instruction
                    });
                }
            }

            // Handle additional product pictures
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

            await _productRepository.UpdateProductAsync(existingProduct);
            var updatedProduct = await _productRepository.GetProductByIdAsync(productId);
            return ProductMapper.ToDTO(updatedProduct);
        }

        public async Task<(List<ProductDTO> Products, int TotalCount)> SearchProductsAsync(ProductSearchRequestDTO request)
        {
            var query = _productRepository.GetQueryable();

            query = query
                .ApplyKeywordFilter(request.Keyword)
                .ApplyCategoryFilter(request.Category)
                .ApplyBrandFilter(request.BrandId)
                .ApplyPriceFilter(request.MinPrice, request.MaxPrice)
                .ApplyMlFilter(request.MinMl, request.MaxMl)
                .ApplySorting(request.SortBy);

            var totalCount = await query.CountAsync();

            var pageNumber = request.PageNumber ?? 1;
            var pageSize = request.PageSize ?? 10;

            var products = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(p => p.Brand)
                .Include(p => p.ProductVariations)
                .Include(p => p.ProductMainIngredients)
                .Include(p => p.ProductForSkinTypes)
                .Include(p => p.ProductPictures)
                .ToListAsync();

            return (products.Select(ProductMapper.ToDTO).ToList(), totalCount);
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            if (product == null)
                return false;

            // Delete main product image if exists
            if (!string.IsNullOrEmpty(product.PictureUrl))
            {
                var fileName = ExtractFilenameFromFirebaseUrl(product.PictureUrl);
                if (!string.IsNullOrEmpty(fileName))
                {
                    await _firebaseService.DeleteImageAsync(fileName);
                }
            }

            // Delete all associated product pictures
            await _productPictureService.DeleteProductPicturesByProductIdAsync(productId);

            await _productRepository.DeleteProductAsync(productId);
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
