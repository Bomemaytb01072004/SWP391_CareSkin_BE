using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Repositories.Interfaces
{
    public interface IBlogNewsRepository
    {
        Task<List<BlogNews>> GetAllNewsAsync();
        Task<BlogNews> GetNewsByIdAsync(int blogId);
        //Task<BlogNew> GetNewsByNameAsync(string title);
        Task AddNewsAsync(BlogNews blog);
        Task UpdateNewsAsync(BlogNews blog);
        Task DeleteNewsAsync(int blogId);

    }
}
