using SWP391_CareSkin_BE.DTOs;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface IHistoryService
    {
        Task<IEnumerable<HistoryDTO>> GetAllHistoriesAsync();
        Task<HistoryDTO> GetHistoryByIdAsync(int id);
        Task<HistoryDTO> CreateHistoryAsync(HistoryDTO historyDto);
        Task<bool> DeleteHistoryAsync(int id);
    }
}
