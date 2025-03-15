using Microsoft.AspNetCore.Http;
using SWP391_CareSkin_BE.DTOS.RatingFeedback;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface IRatingFeedbackService
    {
        Task<IEnumerable<RatingFeedbackDTO>> GetAllRatingFeedbacksAsync();
        Task<IEnumerable<RatingFeedbackDTO>> GetRatingFeedbacksByProductIdAsync(int productId);
        Task<IEnumerable<RatingFeedbackDTO>> GetRatingFeedbacksByCustomerIdAsync(int customerId);
        Task<RatingFeedbackDTO> GetRatingFeedbackByIdAsync(int id);
        Task<RatingFeedbackDTO> CreateRatingFeedbackAsync(int customerId, CreateRatingFeedbackDTO createDto);
        Task<RatingFeedbackDTO> UpdateRatingFeedbackAsync(int customerId, int id, UpdateRatingFeedbackDTO updateDto);
        Task<bool> DeleteRatingFeedbackAsync(int id);
        Task<bool> AdminToggleRatingFeedbackVisibilityAsync(int id, AdminRatingFeedbackActionDTO actionDto);
        Task<bool> AdminDeleteRatingFeedbackAsync(int id);
        Task<double> GetAverageRatingForProductAsync(int productId);
        Task<IEnumerable<RatingFeedbackDTO>> GetActiveRatingFeedbacksAsync();
        Task<IEnumerable<RatingFeedbackDTO>> GetInactiveRatingFeedbacksAsync();
    }
}
