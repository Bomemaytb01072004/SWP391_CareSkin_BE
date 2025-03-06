using SWP391_CareSkin_BE.DTOs.Requests.BlogNews;
using SWP391_CareSkin_BE.DTOs.Responses.BlogNews;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface IBlogNewsService
    {
        Task<List<BlogNewsDTO>> GetAllNewsAsync();
        Task<BlogNewsDTO> GetNewsByIdAsync(int blogId);
        Task<BlogNewsDTO> GetNewsByNameAsync(string title);
        Task<BlogNewsDTO> AddNewsAsync(BlogNewsCreateRequest request, string pictureUrl);
        Task<BlogNewsDTO> UpdateNewsAsync(int blogId, BlogNewsUpdateRequest request);
        Task<bool> DeleteNewsAsync(int blogId);
    }
}
