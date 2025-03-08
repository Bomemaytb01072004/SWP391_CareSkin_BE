using SWP391_CareSkin_BE.DTOs;
using SWP391_CareSkin_BE.DTOs.Requests.History;
using SWP391_CareSkin_BE.DTOs.Responses.History;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface IHistoryService
    {
        List<HistoryResponseDTO> GetAllHistories();
        HistoryResponseDTO GetHistoryById(int historyId);
        void CreateHistory(CreateHistoryRequestDTO dto);
        void UpdateHistory(UpdateHistoryRequestDTO dto);
        void DeleteHistory(int historyId);
    }

}
