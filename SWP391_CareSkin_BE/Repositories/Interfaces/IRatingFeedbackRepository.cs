using SWP391_CareSkin_BE.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391_CareSkin_BE.Repositories.Interfaces
{
    public interface IRatingFeedbackRepository
    {
        Task<IEnumerable<RatingFeedback>> GetAllRatingFeedbacksAsync();
        Task<IEnumerable<RatingFeedback>> GetActiveRatingFeedbacksAsync();
        Task<IEnumerable<RatingFeedback>> GetInactiveRatingFeedbacksAsync();
        Task<IEnumerable<RatingFeedback>> GetRatingFeedbacksByProductIdAsync(int productId);
        Task<IEnumerable<RatingFeedback>> GetRatingFeedbacksByCustomerIdAsync(int customerId);
        Task<RatingFeedback> GetRatingFeedbackByIdAsync(int id);
        Task<RatingFeedback> CreateRatingFeedbackAsync(RatingFeedback ratingFeedback);
        Task<RatingFeedback> UpdateRatingFeedbackAsync(RatingFeedback ratingFeedback);
        Task<double> GetAverageRatingForProductAsync(int productId);
    }
}
