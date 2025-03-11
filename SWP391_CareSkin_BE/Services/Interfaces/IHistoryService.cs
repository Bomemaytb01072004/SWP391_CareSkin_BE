using SWP391_CareSkin_BE.DTOS.Requests.History;
using SWP391_CareSkin_BE.DTOS.Responses.History;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface IHistoryService
    {
        Task<HistoryDTO> CreateHistoryAsync(int attemptId, CreateHistoryDTO createHistoryDTO);
        Task<List<HistoryDTO>> GetHistoriesByAttemptIdAsync(int attemptId, bool includeDetails = false);
        Task<HistoryDTO> GetHistoryByIdAsync(int historyId, bool includeDetails = false);
    }
}
