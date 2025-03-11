using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Repositories.Interfaces
{
    public interface IUserQuizAttemptRepository
    {
        Task<UserQuizAttempt> CreateAsync(UserQuizAttempt userQuizAttempt);
        Task<UserQuizAttempt> GetByIdAsync(int attemptId, bool includeHistories = false);
        Task<IEnumerable<UserQuizAttempt>> GetByCustomerIdAsync(int customerId, bool includeHistories = false);
        Task<IEnumerable<UserQuizAttempt>> GetByQuizAndCustomerAsync(int quizId, int customerId, bool includeHistories = false);
        Task<UserQuizAttempt> UpdateAsync(UserQuizAttempt userQuizAttempt);
        Task<int> GetAttemptNumberAsync(int quizId, int customerId);
        Task<bool> ExistsAsync(int attemptId);
    }
}
