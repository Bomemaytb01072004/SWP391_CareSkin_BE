using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Repositories.Interfaces
{
    public interface IBlogNewsRepository
    {
        Task<List<BlogNew>> GetAllNewsAsync();
        Task<BlogNew> GetNewsByIdAsync(int blogId);
        //Task<BlogNew> GetNewsByNameAsync(string title);
        Task AddNewsAsync(BlogNew blog);
        Task UpdateNewsAsync(BlogNew blog);
        Task DeleteNewsAsync(int blogId);

    }
}
