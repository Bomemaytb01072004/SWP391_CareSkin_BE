using System;
using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;

namespace SWP391_CareSkin_BE.Repositories.Implementations
{
    public class FAQRepository : IFAQRepository
    {
        private readonly MyDbContext _context;

        public FAQRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<List<FAQ>> GetAllFAQsAsync()
        {
            return await _context.FAQs.ToListAsync();
        }

        public async Task<FAQ?> GetFAQByIdAsync(int faqId)
        {
            return await _context.FAQs.FindAsync(faqId);
        }

        public async Task AddFAQAsync(FAQ faq)
        {
            await _context.FAQs.AddAsync(faq);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateFAQAsync(FAQ faq)
        {
            _context.FAQs.Update(faq);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFAQAsync(FAQ faq)
        {
            _context.FAQs.Remove(faq);
            await _context.SaveChangesAsync();
        }
    }
}
