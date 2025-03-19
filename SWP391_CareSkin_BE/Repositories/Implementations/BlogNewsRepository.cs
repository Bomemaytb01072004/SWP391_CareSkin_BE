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

        public async Task<List<BlogNews>> GetAllNewsAsync()
        {
            return await _context.BlogNews.ToListAsync();
        }

        public async Task<BlogNews> GetNewsByIdAsync(int blogId)
        {
            return await _context.BlogNews.FirstOrDefaultAsync(id  => id.BlogId == blogId);
        }

        public async Task AddNewsAsync(BlogNews blog)
        {
            _context.BlogNews.AddAsync(blog);
            await _context.SaveChangesAsync();
        }
        
        public async Task UpdateNewsAsync(BlogNews blog)
        {
            _context.BlogNews.Update(blog);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteNewsAsync(int blogId)
        {
            var blog = await GetNewsByIdAsync(blogId);
            if (blog != null)
            {
                blog.IsActive = false; 
                _context.BlogNews.Update(blog);
                await _context.SaveChangesAsync();
            }
        }


    }
}
