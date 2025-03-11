using SWP391_CareSkin_BE.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391_CareSkin_BE.Repositories.Interfaces
{
    public interface IHistoryRepository
    {
        Task<History> CreateHistory(History history);
        Task<History> UpdateHistory(History history);
        Task<History> GetHistoryById(int historyId);
        Task<IEnumerable<History>> GetHistoryByUserId(int userId);
        Task<History> DeleteHistory(int historyId);
        Task<History> GetHistoryByAttemptAndQuestion(int attemptId, int questionId);
        Task<IEnumerable<History>> GetHistoriesByAttemptId(int attemptId);
    }
}
