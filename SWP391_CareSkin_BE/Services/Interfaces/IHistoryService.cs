using SWP391_CareSkin_BE.DTOs;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface IHistoryService
    {
        Task<History> SaveHistoryAsync(HistoryDTO dto);
    }
}
