using SWP391_CareSkin_BE.DTOs;
using SWP391_CareSkin_BE.DTOs.Requests.History;
using SWP391_CareSkin_BE.DTOs.Responses.History;
using SWP391_CareSkin_BE.Mappers;
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

        public List<HistoryResponseDTO> GetAllHistories()
        {
            var histories = _historyRepository.GetAllHistories();
            return histories.Select(HistoryMapper.ToDTO).ToList();
        }

        public HistoryResponseDTO GetHistoryById(int historyId)
        {
            var history = _historyRepository.GetHistoryById(historyId);
            return history != null ? HistoryMapper.ToDTO(history) : null;
        }

        public void CreateHistory(CreateHistoryRequestDTO dto)
        {
            var history = HistoryMapper.ToEntity(dto);
            _historyRepository.AddHistory(history);
        }

        public void UpdateHistory(UpdateHistoryRequestDTO dto)
        {
            var history = _historyRepository.GetHistoryById(dto.HistoryId);
            if (history != null)
            {
                history.CustomerId = dto.CustomerId;
                history.QuestionId = dto.QuestionId;
                history.AnswerId = dto.AnswerId;
                _historyRepository.UpdateHistory(history);
            }
        }

        public void DeleteHistory(int historyId)
        {
            var history = _historyRepository.GetHistoryById(historyId);
            if (history == null)
                throw new Exception("History not found");

            _historyRepository.DeleteHistory(historyId);
        }
    }
}
