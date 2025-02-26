using SWP391_CareSkin_BE.DTOs.Responses;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface IFAQService
    {
        Task<List<ShowFAQDTO>> GetAllFAQsAsync();

       
    }
}
