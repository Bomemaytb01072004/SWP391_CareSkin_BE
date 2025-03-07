using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP391_CareSkin_BE.Repositories.Implementations
{
    public class RatingFeedbackRepository : IRatingFeedbackRepository
    {
        private readonly MyDbContext _context;

        public RatingFeedbackRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RatingFeedback>> GetAllRatingFeedbacksAsync()
        {
            return await _context.RatingFeedbacks
                .Include(rf => rf.Customer)
                .Include(rf => rf.Product)
                .Include(rf => rf.RatingFeedbackImages)
                .ToListAsync();
        }

        public async Task<IEnumerable<RatingFeedback>> GetRatingFeedbacksByProductIdAsync(int productId)
        {
            return await _context.RatingFeedbacks
                .Include(rf => rf.Customer)
                .Include(rf => rf.Product)
                .Include(rf => rf.RatingFeedbackImages)
                .Where(rf => rf.ProductId == productId)
                .ToListAsync();
        }

        public async Task<IEnumerable<RatingFeedback>> GetRatingFeedbacksByCustomerIdAsync(int customerId)
        {
            return await _context.RatingFeedbacks
                .Include(rf => rf.Product)
                .Include(rf => rf.RatingFeedbackImages)
                .Where(rf => rf.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task<RatingFeedback> GetRatingFeedbackByIdAsync(int id)
        {
            return await _context.RatingFeedbacks
                .Include(rf => rf.Customer)
                .Include(rf => rf.Product)
                    .ThenInclude(p => p.ProductName)
                .Include(rf => rf.RatingFeedbackImages)
                .FirstOrDefaultAsync(rf => rf.Id == id);
        }

        public async Task<RatingFeedback> CreateRatingFeedbackAsync(RatingFeedback ratingFeedback)
        {
            _context.RatingFeedbacks.Add(ratingFeedback);
            await _context.SaveChangesAsync();
            return ratingFeedback;
        }

        public async Task<RatingFeedback> UpdateRatingFeedbackAsync(RatingFeedback ratingFeedback)
        {
            _context.Entry(ratingFeedback).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return ratingFeedback;
        }

        public async Task<bool> DeleteRatingFeedbackAsync(int id)
        {
            var ratingFeedback = await _context.RatingFeedbacks.FindAsync(id);
            if (ratingFeedback == null)
                return false;

            _context.RatingFeedbacks.Remove(ratingFeedback);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ToggleRatingFeedbackVisibilityAsync(int id, bool isVisible)
        {
            var ratingFeedback = await _context.RatingFeedbacks.FindAsync(id);
            if (ratingFeedback == null)
                return false;

            ratingFeedback.IsVisible = isVisible;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CustomerOwnsRatingFeedbackAsync(int customerId, int ratingFeedbackId)
        {
            return await _context.RatingFeedbacks
                .AnyAsync(rf => rf.Id == ratingFeedbackId && rf.CustomerId == customerId);
        }

        public async Task<double> GetAverageRatingForProductAsync(int productId)
        {
            var ratings = await _context.RatingFeedbacks
                .Where(rf => rf.ProductId == productId && rf.IsVisible)
                .Select(rf => rf.Rating)
                .ToListAsync();

            if (ratings == null || !ratings.Any())
                return 0;

            return ratings.Average();
        }
    }
}
