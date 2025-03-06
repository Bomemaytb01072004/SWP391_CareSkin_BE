using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;

namespace SWP391_CareSkin_BE.Repositories.Implementations
{
    public class BlogNewsRepository : IBlogNewsRepository
    {
        private readonly MyDbContext _context;

        public BlogNewsRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<List<BlogNew>> GetAllNewsAsync()
        {
            return await _context.BlogNews.ToListAsync();
        }

        public async Task<BlogNew> GetNewsByIdAsync(int blogId)
        {
            return await _context.BlogNews.FirstOrDefaultAsync(id  => id.BlogId == blogId);
        }

        public async Task<BlogNew> GetNewsByNameAsync(string title)
        {
            return await _context.BlogNews.FirstOrDefaultAsync(blg => blg.Title == title);
        }

        public async Task AddNewsAsync(BlogNew blog)
        {
            _context.BlogNews.Add(blog);
            await _context.SaveChangesAsync();
        }
        
        public async Task UpdateNewsAsync(BlogNew blog)
        {
            _context.BlogNews.Update(blog);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteNewsAsync(int blogId)
        {
            var blog = await GetNewsByIdAsync(blogId);
            if (blog != null)
            {
                _context.BlogNews.Remove(blog);
                await _context.SaveChangesAsync();
            }
        }


    }
}
