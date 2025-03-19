using SWP391_CareSkin_BE.DTOs.Requests.BlogNews;
using SWP391_CareSkin_BE.DTOs.Responses.BlogNews;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface IBlogNewsService
    {
        Task<List<BlogNewsDTO>> GetAllNewsAsync();
        Task<BlogNewsDTO> GetNewsByIdAsync(int blogId);
        //Task<BlogNewsDTO> GetNewsByNameAsync(string title);
        Task<BlogNewsDTO> AddNewsAsync(BlogNewsCreateRequest request, DateTime date, string pictureUrl, int? adminId, int? staffId);
        Task<BlogNewsDTO> UpdateNewsAsync(int blogId, BlogNewsUpdateRequest request, string pictureUrl);
        Task<bool> DeleteNewsAsync(int blogId);
    }
}
