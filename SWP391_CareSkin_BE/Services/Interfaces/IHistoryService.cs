using SWP391_CareSkin_BE.DTOS.Requests.History;
using SWP391_CareSkin_BE.DTOS.Responses.History;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface IHistoryService
    {
        Task<List<HistoryDTO>> CreateHistoriesAsync(int attemptId, List<CreateHistoryDTO> createHistoryDTOs);
        Task<HistoryDTO> CreateOrUpdateHistoryAsync(int attemptId, CreateHistoryDTO historyDTO);
        Task<List<HistoryDTO>> GetHistoriesByAttemptIdAsync(int attemptId, bool includeDetails = false);
        Task<HistoryDTO> GetHistoryByIdAsync(int historyId, bool includeDetails = false);
    }
}
