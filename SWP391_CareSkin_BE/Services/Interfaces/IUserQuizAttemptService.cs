using SWP391_CareSkin_BE.DTOS.Requests.UserQuizAttempt;
using SWP391_CareSkin_BE.DTOS.Responses.UserQuizAttempt;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface IUserQuizAttemptService
    {
        Task<UserQuizAttemptDTO> CreateUserQuizAttemptAsync(CreateUserQuizAttemptDTO createUserQuizAttemptDTO);
        Task<UserQuizAttemptDTO> GetUserQuizAttemptByIdAsync(int attemptId, bool includeHistories = false);
        Task<List<UserQuizAttemptDTO>> GetUserQuizAttemptsByCustomerIdAsync(int customerId, bool includeHistories = false);
        Task<List<UserQuizAttemptDTO>> GetUserQuizAttemptsByQuizAndCustomerAsync(int quizId, int customerId, bool includeHistories = false);
        Task<UserQuizAttemptDTO> CompleteUserQuizAttemptAsync(int attemptId);
    }
}
