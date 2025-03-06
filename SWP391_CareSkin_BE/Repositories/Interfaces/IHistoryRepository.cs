using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Repositories.Interfaces
{
    public interface IHistoryRepository
    {
        Task<History> AddAsync(History history);
    }
}
