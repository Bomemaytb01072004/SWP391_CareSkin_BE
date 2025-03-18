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

        public async Task<IEnumerable<RatingFeedbackDTO>> GetActiveRatingFeedbacksAsync()
        {
            var ratingFeedbacks = await _ratingFeedbackRepository.GetActiveRatingFeedbacksAsync();
            return RatingFeedbackMapper.ToDTOList(ratingFeedbacks);
        }

        public async Task<IEnumerable<RatingFeedbackDTO>> GetInactiveRatingFeedbacksAsync()
        {
            var ratingFeedbacks = await _ratingFeedbackRepository.GetInactiveRatingFeedbacksAsync();
            return RatingFeedbackMapper.ToDTOList(ratingFeedbacks);
        }

        public async Task<IEnumerable<RatingFeedbackDTO>> GetRatingFeedbacksByProductIdAsync(int productId)
        {
            var ratingFeedbacks = await _ratingFeedbackRepository.GetRatingFeedbacksByProductIdAsync(productId);
            return RatingFeedbackMapper.ToDTOList(ratingFeedbacks.Where(rf => rf.IsActive));
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
            // Kiểm tra xem đánh giá của khách hàng cho sản phẩm này đã tồn tại và đang active chưa
            var existingRatingFeedback = await _ratingFeedbackRepository.GetRatingFeedbackByCustomerAndProductAsync(customerId, createDto.ProductId);
            
            if (existingRatingFeedback != null && existingRatingFeedback.IsActive)
            {
                throw new ArgumentException($"Bạn đã đánh giá sản phẩm này rồi.");
            }

            // Use the mapper to create the entity
            var ratingFeedback = RatingFeedbackMapper.ToEntity(customerId, createDto);

            await _ratingFeedbackRepository.CreateRatingFeedbackAsync(ratingFeedback);

            // Process images if any
            if (createDto.FeedbackImages != null && createDto.FeedbackImages.Count > 0)
            {
                foreach (var image in createDto.FeedbackImages)
                {
                    if (image != null && image.Length > 0)
                    {
                        var fileName = $"{Guid.NewGuid()}_{image.FileName}";
                        using var stream = image.OpenReadStream();
                        var imageUrl = await _firebaseService.UploadImageAsync(stream, fileName);

                        var ratingFeedbackImage = new RatingFeedbackImage
                        {
                            RatingFeedbackId = ratingFeedback.RatingFeedbackId,
                            FeedbackImageUrl = imageUrl
                        };

                        await _ratingFeedbackImageRepository.CreateImageAsync(ratingFeedbackImage);
                    }
                }
            }

            // Reload the entity with images
            var createdRatingFeedback = await _ratingFeedbackRepository.GetRatingFeedbackByIdAsync(ratingFeedback.RatingFeedbackId);

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

            // Process images before updating the entity
            
            // Delete specific images if requested
            if (updateDto.ImagesToDelete != null && updateDto.ImagesToDelete.Any())
            {
                foreach (var imageId in updateDto.ImagesToDelete)
                {
                    var image = await _ratingFeedbackImageRepository.GetImageByIdAsync(imageId);
                    if (image != null && image.RatingFeedbackId == id)
                    {
                        // Delete from storage if using Firebase
                        if (!string.IsNullOrEmpty(image.FeedbackImageUrl))
                        {
                            var fileName = ExtractFilenameFromFirebaseUrl(image.FeedbackImageUrl);
                            if (!string.IsNullOrEmpty(fileName))
                            {
                                await _firebaseService.DeleteImageAsync(fileName);
                            }
                        }
                        
                        // Delete from database
                        await _ratingFeedbackImageRepository.DeleteImageAsync(imageId);
                    }
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
                            RatingFeedbackId = ratingFeedback.RatingFeedbackId,
                            FeedbackImageUrl = imageUrl
                        };

                        await _ratingFeedbackImageRepository.CreateImageAsync(ratingFeedbackImage);
                    }
                }
            }

            // Now update the entity after processing images
            await _ratingFeedbackRepository.UpdateRatingFeedbackAsync(ratingFeedback);

            // Reload the entity with images
            var updatedRatingFeedback = await _ratingFeedbackRepository.GetRatingFeedbackByIdAsync(id);

            // Update product average rating
            await UpdateProductAverageRatingAsync(ratingFeedback.ProductId);

            return RatingFeedbackMapper.ToDTO(updatedRatingFeedback);
        }

        public async Task<bool> DeleteRatingFeedbackAsync(int id)
        {
            var ratingFeedback = await _ratingFeedbackRepository.GetRatingFeedbackByIdAsync(id);
            //if (ratingFeedback == null || ratingFeedback.CustomerId != customerId)
            //    return false;

            if (ratingFeedback == null)
                return false;

            var productId = ratingFeedback.ProductId;

            // Implement soft delete by setting IsActive to false
            ratingFeedback.IsActive = false;
            await _ratingFeedbackRepository.UpdateRatingFeedbackAsync(ratingFeedback);

            // Update product average rating
            await UpdateProductAverageRatingAsync(productId);

            return true;
        }

        public async Task<bool> AdminToggleRatingFeedbackVisibilityAsync(int id, AdminRatingFeedbackActionDTO actionDto)
        {
            var ratingFeedback = await _ratingFeedbackRepository.GetRatingFeedbackByIdAsync(id);
            if (ratingFeedback == null)
                return false;

            // Store the product ID for rating recalculation
            var productId = ratingFeedback.ProductId;

            // Update the rating feedback status
            ratingFeedback.IsActive = actionDto.IsActive;
            ratingFeedback.UpdatedDate = DateTime.UtcNow;

            await _ratingFeedbackRepository.UpdateRatingFeedbackAsync(ratingFeedback);

            // Update product average rating
            await UpdateProductAverageRatingAsync(productId);

            return true;
        }

        public async Task<bool> AdminDeleteRatingFeedbackAsync(int id)
        {
            var ratingFeedback = await _ratingFeedbackRepository.GetRatingFeedbackByIdAsync(id);
            if (ratingFeedback == null)
                return false;

            // Store the product ID for rating recalculation
            var productId = ratingFeedback.ProductId;

            // Soft delete by setting IsActive to false
            ratingFeedback.IsActive = false;
            ratingFeedback.UpdatedDate = DateTime.UtcNow;

            // Update the rating feedback
            var result = await _ratingFeedbackRepository.UpdateRatingFeedbackAsync(ratingFeedback);

            // Update product average rating
            if (result != null)
            {
                await UpdateProductAverageRatingAsync(productId);
                Console.WriteLine($"Admin deleted rating {id} for product {productId}, recalculated average rating");
            }

            return true;
        }

        public async Task<double> GetAverageRatingForProductAsync(int productId)
        {
            return await _ratingFeedbackRepository.GetAverageRatingForProductAsync(productId);
        }

        private async Task UpdateProductAverageRatingAsync(int productId)
        {
            try
            {
                // Get the average rating from active ratings only
                var averageRating = await _ratingFeedbackRepository.GetAverageRatingForProductAsync(productId);
                
                // Get the product
                var product = await _productRepository.GetProductByIdAsync(productId);

                if (product != null)
                {
                    // Update the product's average rating
                    product.AverageRating = averageRating;
                    
                    // Save the changes
                    await _productRepository.UpdateProductAsync(product);
                    
                    // Log the update
                    Console.WriteLine($"Updated product {productId} average rating to {averageRating}");
                }
                else
                {
                    Console.WriteLine($"Product {productId} not found when updating average rating");
                }
            }
            catch (Exception ex)
            {
                // Log any errors
                Console.WriteLine($"Error updating product {productId} average rating: {ex.Message}");
            }
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
