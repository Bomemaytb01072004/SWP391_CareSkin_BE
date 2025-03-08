using SWP391_CareSkin_BE.DTOs;

namespace SWP391_CareSkin_BE.Repositories.Interfaces
{
    public interface IHistoryRepository
    {
        Task<IEnumerable<HistoryDTO>> GetAllAsync();
        Task<HistoryDTO> GetByIdAsync(int id);
        Task<HistoryDTO> AddAsync(HistoryDTO historyDto);
        Task<bool> DeleteHistoryAsync(int id); 
    }
}
