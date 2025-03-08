using SWP391_CareSkin_BE.DTOs;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Services.Implementations
{
    public class HistoryService : IHistoryService
    {
        private readonly IHistoryRepository _historyRepository;

        public HistoryService(IHistoryRepository historyRepository)
        {
            _historyRepository = historyRepository;
        }

        public async Task<IEnumerable<HistoryDTO>> GetAllHistoriesAsync()
        {
            return await _historyRepository.GetAllAsync();
        }

        public async Task<HistoryDTO> GetHistoryByIdAsync(int id)
        {
            return await _historyRepository.GetByIdAsync(id);
        }

        public async Task<HistoryDTO> CreateHistoryAsync(HistoryDTO historyDto)
        {
            return await _historyRepository.AddAsync(historyDto);
        }

        public async Task<bool> DeleteHistoryAsync(int id)
        {
            return await _historyRepository.DeleteHistoryAsync(id);
        }

    }
}
