using SWP391_CareSkin_BE.DTOs;
using SWP391_CareSkin_BE.DTOs.Requests.History;
using SWP391_CareSkin_BE.DTOs.Responses.History;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Repositories.Interfaces
{
    public interface IHistoryRepository
    {
        List<History> GetAllHistories();
        History GetHistoryById(int historyId);
        void AddHistory(History history);
        void UpdateHistory(History history);
        void DeleteHistory(int historyId);
        void SaveChanges();
    }


}
