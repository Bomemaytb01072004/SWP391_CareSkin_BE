using SWP391_CareSkin_BE.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391_CareSkin_BE.Repositories.Interfaces
{
    public interface IRatingFeedbackImageRepository
    {
        Task<IEnumerable<RatingFeedbackImage>> GetImagesByRatingFeedbackIdAsync(int ratingFeedbackId);
        Task<RatingFeedbackImage> GetImageByIdAsync(int id);
        Task<RatingFeedbackImage> CreateImageAsync(RatingFeedbackImage image);
        Task<bool> DeleteImageAsync(int id);
        Task<bool> DeleteImagesByRatingFeedbackIdAsync(int ratingFeedbackId);
    }
}
