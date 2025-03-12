using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP391_CareSkin_BE.Repositories.Implementations
{
    public class RatingFeedbackImageRepository : IRatingFeedbackImageRepository
    {
        private readonly MyDbContext _context;

        public RatingFeedbackImageRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RatingFeedbackImage>> GetImagesByRatingFeedbackIdAsync(int ratingFeedbackId)
        {
            return await _context.RatingFeedbackImages
                .Where(image => image.RatingFeedbackId == ratingFeedbackId)
                .ToListAsync();
        }

        public async Task<RatingFeedbackImage> GetImageByIdAsync(int id)
        {
            return await _context.RatingFeedbackImages.FindAsync(id);
        }

        public async Task<RatingFeedbackImage> CreateImageAsync(RatingFeedbackImage image)
        {
            _context.RatingFeedbackImages.Add(image);
            await _context.SaveChangesAsync();
            return image;
        }

        public async Task<bool> DeleteImageAsync(int id)
        {
            var image = await _context.RatingFeedbackImages.FindAsync(id);
            if (image == null)
                return false;

            _context.RatingFeedbackImages.Remove(image);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteImagesByRatingFeedbackIdAsync(int ratingFeedbackId)
        {
            var images = await _context.RatingFeedbackImages
                .Where(image => image.RatingFeedbackId == ratingFeedbackId)
                .ToListAsync();

            if (images == null || !images.Any())
                return false;

            _context.RatingFeedbackImages.RemoveRange(images);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
