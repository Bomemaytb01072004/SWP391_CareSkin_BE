using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SWP391_CareSkin_BE.DTOS.ProductPicture;
using SWP391_CareSkin_BE.Mappers;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using SWP391_CareSkin_BE.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP391_CareSkin_BE.Services.Implementations
{
    public class ProductPictureService : IProductPictureService
    {
        private readonly IProductPictureRepository _productPictureRepository;
        private readonly IProductRepository _productRepository;
        private readonly IFirebaseService _firebaseService;

        public ProductPictureService(
            IProductPictureRepository productPictureRepository,
            IProductRepository productRepository,
            IFirebaseService firebaseService)
        {
            _productPictureRepository = productPictureRepository;
            _productRepository = productRepository;
            _firebaseService = firebaseService;
        }

        public async Task<IEnumerable<ProductPictureDTO>> GetAllProductPicturesAsync()
        {
            var productPictures = await _productPictureRepository.GetAllProductPicturesAsync();
            return ProductPictureMapper.ToDTOList(productPictures);
        }

        public async Task<IEnumerable<ProductPictureDTO>> GetProductPicturesByProductIdAsync(int productId)
        {
            var productPictures = await _productPictureRepository.GetProductPicturesByProductIdAsync(productId);
            return ProductPictureMapper.ToDTOList(productPictures);
        }

        public async Task<ProductPictureDTO> GetProductPictureByIdAsync(int id)
        {
            var productPicture = await _productPictureRepository.GetProductPictureByIdAsync(id);
            return ProductPictureMapper.ToDTO(productPicture);
        }

        public async Task<ProductPictureDTO> CreateProductPictureAsync(CreateProductPictureDTO createDto)
        {
            // Check if product exists
            var product = await _productRepository.GetProductByIdAsync(createDto.ProductId);
            if (product == null)
                return null;

            // Use the mapper to create the entity
            var productPicture = ProductPictureMapper.ToEntity(createDto);

            // Process image
            if (createDto.Image != null && createDto.Image.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}_{createDto.Image.FileName}";
                using var stream = createDto.Image.OpenReadStream();
                var imageUrl = await _firebaseService.UploadImageAsync(stream, fileName);
                productPicture.PictureUrl = imageUrl;
            }

            await _productPictureRepository.CreateProductPictureAsync(productPicture);

            // Reload the entity
            var createdProductPicture = await _productPictureRepository.GetProductPictureByIdAsync(productPicture.ProductPictureId);
            return ProductPictureMapper.ToDTO(createdProductPicture);
        }

        public async Task<ProductPictureDTO> UpdateProductPictureAsync(int id, UpdateProductPictureDTO updateDto)
        {
            var productPicture = await _productPictureRepository.GetProductPictureByIdAsync(id);
            if (productPicture == null)
                return null;

            // Process image
            if (updateDto.Image != null && updateDto.Image.Length > 0)
            {
                // Delete old image from Firebase if it exists
                if (!string.IsNullOrEmpty(productPicture.PictureUrl))
                {
                    var oldFileName = ExtractFilenameFromFirebaseUrl(productPicture.PictureUrl);
                    if (!string.IsNullOrEmpty(oldFileName))
                    {
                        await _firebaseService.DeleteImageAsync(oldFileName);
                    }
                }

                // Upload new image
                var fileName = $"{Guid.NewGuid()}_{updateDto.Image.FileName}";
                using var stream = updateDto.Image.OpenReadStream();
                var imageUrl = await _firebaseService.UploadImageAsync(stream, fileName);
                
                // Update the entity
                ProductPictureMapper.UpdateEntity(productPicture, imageUrl);
            }

            await _productPictureRepository.UpdateProductPictureAsync(productPicture);

            // Reload the entity
            var updatedProductPicture = await _productPictureRepository.GetProductPictureByIdAsync(id);
            return ProductPictureMapper.ToDTO(updatedProductPicture);
        }

        public async Task<bool> DeleteProductPictureAsync(int id)
        {
            var productPicture = await _productPictureRepository.GetProductPictureByIdAsync(id);
            if (productPicture == null)
                return false;

            // Delete image from Firebase if it exists
            if (!string.IsNullOrEmpty(productPicture.PictureUrl))
            {
                var fileName = ExtractFilenameFromFirebaseUrl(productPicture.PictureUrl);
                if (!string.IsNullOrEmpty(fileName))
                {
                    await _firebaseService.DeleteImageAsync(fileName);
                }
            }

            return await _productPictureRepository.DeleteProductPictureAsync(id);
        }

        public async Task<bool> DeleteProductPicturesByProductIdAsync(int productId)
        {
            var productPictures = await _productPictureRepository.GetProductPicturesByProductIdAsync(productId);
            if (productPictures == null || !productPictures.Any())
                return false;

            // Delete all images from Firebase
            foreach (var picture in productPictures)
            {
                if (!string.IsNullOrEmpty(picture.PictureUrl))
                {
                    var fileName = ExtractFilenameFromFirebaseUrl(picture.PictureUrl);
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        await _firebaseService.DeleteImageAsync(fileName);
                    }
                }
            }

            return await _productPictureRepository.DeleteProductPicturesByProductIdAsync(productId);
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
