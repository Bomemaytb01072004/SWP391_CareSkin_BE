using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SWP391_CareSkin_BE.DTOS.RatingFeedback;
using SWP391_CareSkin_BE.Mappers;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using SWP391_CareSkin_BE.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SWP391_CareSkin_BE.Services.Implementations
{
    public class RatingFeedbackService : IRatingFeedbackService
    {
        private readonly IRatingFeedbackRepository _ratingFeedbackRepository;
        private readonly IRatingFeedbackImageRepository _ratingFeedbackImageRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IProductRepository _productRepository;
        private readonly IFirebaseService _firebaseService;

        public RatingFeedbackService(
            IRatingFeedbackRepository ratingFeedbackRepository,
            IRatingFeedbackImageRepository ratingFeedbackImageRepository,
            IWebHostEnvironment webHostEnvironment,
            IProductRepository productRepository,
            IFirebaseService firebaseService)
        {
            _ratingFeedbackRepository = ratingFeedbackRepository;
            _ratingFeedbackImageRepository = ratingFeedbackImageRepository;
            _webHostEnvironment = webHostEnvironment;
            _productRepository = productRepository;
            _firebaseService = firebaseService;
        }

        public async Task<IEnumerable<RatingFeedbackDTO>> GetAllRatingFeedbacksAsync()
        {
            var ratingFeedbacks = await _ratingFeedbackRepository.GetAllRatingFeedbacksAsync();
            return RatingFeedbackMapper.ToDTOList(ratingFeedbacks);
        }

        public async Task<IEnumerable<RatingFeedbackDTO>> GetRatingFeedbacksByProductIdAsync(int productId)
        {
            var ratingFeedbacks = await _ratingFeedbackRepository.GetRatingFeedbacksByProductIdAsync(productId);
            return RatingFeedbackMapper.ToDTOList(ratingFeedbacks.Where(rf => rf.IsVisible));
        }

        public async Task<IEnumerable<RatingFeedbackDTO>> GetRatingFeedbacksByCustomerIdAsync(int customerId)
        {
            var ratingFeedbacks = await _ratingFeedbackRepository.GetRatingFeedbacksByCustomerIdAsync(customerId);
            return RatingFeedbackMapper.ToDTOList(ratingFeedbacks);
        }

        public async Task<RatingFeedbackDTO> GetRatingFeedbackByIdAsync(int id)
        {
            var ratingFeedback = await _ratingFeedbackRepository.GetRatingFeedbackByIdAsync(id);
            return RatingFeedbackMapper.ToDTO(ratingFeedback);
        }

        public async Task<RatingFeedbackDTO> CreateRatingFeedbackAsync(int customerId, CreateRatingFeedbackDTO createDto)
        {
            // Use the mapper to create the entity
            var ratingFeedback = RatingFeedbackMapper.ToEntity(customerId, createDto);

            await _ratingFeedbackRepository.CreateRatingFeedbackAsync(ratingFeedback);

            // Process images if any
            if (createDto.Images != null && createDto.Images.Count > 0)
            {
                foreach (var image in createDto.Images)
                {
                    if (image != null && image.Length > 0)
                    {
                        var fileName = $"{Guid.NewGuid()}_{image.FileName}";
                        using var stream = image.OpenReadStream();
                        var imageUrl = await _firebaseService.UploadImageAsync(stream, fileName);

                        var ratingFeedbackImage = new RatingFeedbackImage
                        {
                            RatingFeedbackId = ratingFeedback.Id,
                            ImageUrl = imageUrl
                        };

                        await _ratingFeedbackImageRepository.CreateImageAsync(ratingFeedbackImage);
                    }
                }
            }

            // Reload the entity with images
            var createdRatingFeedback = await _ratingFeedbackRepository.GetRatingFeedbackByIdAsync(ratingFeedback.Id);

            // Update product average rating
            await UpdateProductAverageRatingAsync(createDto.ProductId);

            return RatingFeedbackMapper.ToDTO(createdRatingFeedback);
        }

        public async Task<RatingFeedbackDTO> UpdateRatingFeedbackAsync(int customerId, int id, UpdateRatingFeedbackDTO updateDto)
        {
            var ratingFeedback = await _ratingFeedbackRepository.GetRatingFeedbackByIdAsync(id);
            if (ratingFeedback == null || ratingFeedback.CustomerId != customerId)
                return null;

            // Use the mapper to update the entity
            RatingFeedbackMapper.UpdateEntity(ratingFeedback, updateDto);

            await _ratingFeedbackRepository.UpdateRatingFeedbackAsync(ratingFeedback);

            // Delete specific images if requested
            if (updateDto.ImagesToDelete != null && updateDto.ImagesToDelete.Any())
            {
                foreach (var imageId in updateDto.ImagesToDelete)
                {
                    await _ratingFeedbackImageRepository.DeleteImageAsync(imageId);
                }
            }

            // Process new images if any
            if (updateDto.NewImages != null && updateDto.NewImages.Count > 0)
            {
                foreach (var image in updateDto.NewImages)
                {
                    if (image != null && image.Length > 0)
                    {
                        var fileName = $"{Guid.NewGuid()}_{image.FileName}";
                        using var stream = image.OpenReadStream();
                        var imageUrl = await _firebaseService.UploadImageAsync(stream, fileName);

                        var ratingFeedbackImage = new RatingFeedbackImage
                        {
                            RatingFeedbackId = ratingFeedback.Id,
                            ImageUrl = imageUrl
                        };

                        await _ratingFeedbackImageRepository.CreateImageAsync(ratingFeedbackImage);
                    }
                }
            }

            // Reload the entity with images
            var updatedRatingFeedback = await _ratingFeedbackRepository.GetRatingFeedbackByIdAsync(id);

            // Update product average rating
            await UpdateProductAverageRatingAsync(ratingFeedback.ProductId);

            return RatingFeedbackMapper.ToDTO(updatedRatingFeedback);
        }

        public async Task<bool> DeleteRatingFeedbackAsync(int customerId, int id)
        {
            var ratingFeedback = await _ratingFeedbackRepository.GetRatingFeedbackByIdAsync(id);
            if (ratingFeedback == null || ratingFeedback.CustomerId != customerId)
                return false;

            var productId = ratingFeedback.ProductId;

            // Delete all associated images
            await _ratingFeedbackImageRepository.DeleteImagesByRatingFeedbackIdAsync(id);

            // Delete the rating feedback
            var result = await _ratingFeedbackRepository.DeleteRatingFeedbackAsync(id);

            // Update product average rating
            if (result)
                await UpdateProductAverageRatingAsync(productId);

            return result;
        }

        public async Task<bool> AdminToggleRatingFeedbackVisibilityAsync(int id, AdminRatingFeedbackActionDTO actionDto)
        {
            var ratingFeedback = await _ratingFeedbackRepository.GetRatingFeedbackByIdAsync(id);
            if (ratingFeedback == null)
                return false;

            ratingFeedback.IsVisible = actionDto.IsVisible;
            ratingFeedback.UpdatedDate = DateTime.UtcNow;

            await _ratingFeedbackRepository.UpdateRatingFeedbackAsync(ratingFeedback);

            // Update product average rating
            await UpdateProductAverageRatingAsync(ratingFeedback.ProductId);

            return true;
        }

        public async Task<bool> AdminDeleteRatingFeedbackAsync(int id)
        {
            var ratingFeedback = await _ratingFeedbackRepository.GetRatingFeedbackByIdAsync(id);
            if (ratingFeedback == null)
                return false;

            var productId = ratingFeedback.ProductId;

            // Delete all associated images
            await _ratingFeedbackImageRepository.DeleteImagesByRatingFeedbackIdAsync(id);

            // Delete the rating feedback
            var result = await _ratingFeedbackRepository.DeleteRatingFeedbackAsync(id);

            // Update product average rating
            if (result)
                await UpdateProductAverageRatingAsync(productId);

            return result;
        }

        public async Task<double> GetAverageRatingForProductAsync(int productId)
        {
            return await _ratingFeedbackRepository.GetAverageRatingForProductAsync(productId);
        }

        private async Task UpdateProductAverageRatingAsync(int productId)
        {
            var averageRating = await _ratingFeedbackRepository.GetAverageRatingForProductAsync(productId);
            var product = await _productRepository.GetProductByIdAsync(productId);

            if (product != null)
            {
                product.AverageRating = averageRating;
                await _productRepository.UpdateProductAsync(product);
            }
        }
    }
}
