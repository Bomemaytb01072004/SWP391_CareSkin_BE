using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391_CareSkin_BE.Repositories.Implementations
{
    public class ResultRepository : IResultRepository
    {
        private readonly MyDbContext _context;

        public ResultRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<Result> CreateAsync(Result result)
        {
            _context.Results.Add(result);
            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<Result> UpdateAsync(Result result)
        {
            _context.Entry(result).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<Result> GetByIdAsync(int resultId)
        {
            return await _context.Results
                .Include(r => r.SkinType)
                .Include(r => r.UserQuizAttempt)
                .FirstOrDefaultAsync(r => r.ResultId == resultId);
        }

        public async Task<IEnumerable<Result>> GetByCustomerIdAsync(int customerId)
        {
            return await _context.Results
                .Include(r => r.SkinType)
                .Include(r => r.UserQuizAttempt)
                .Where(r => r.CustomerId == customerId)
                .OrderByDescending(r => r.LastQuizTime)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int resultId)
        {
            return await _context.Results.AnyAsync(r => r.ResultId == resultId);
        }
    }
}
