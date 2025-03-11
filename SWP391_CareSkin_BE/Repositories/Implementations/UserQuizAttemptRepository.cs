using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;

namespace SWP391_CareSkin_BE.Repositories.Implementations
{
    public class UserQuizAttemptRepository : IUserQuizAttemptRepository
    {
        private readonly MyDbContext _context;

        public UserQuizAttemptRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<UserQuizAttempt> CreateAsync(UserQuizAttempt userQuizAttempt)
        {
            _context.UserQuizAttempts.Add(userQuizAttempt);
            await _context.SaveChangesAsync();
            return userQuizAttempt;
        }

        public async Task<UserQuizAttempt> GetByIdAsync(int attemptId, bool includeHistories = false)
        {
            var query = _context.UserQuizAttempts.AsQueryable();

            if (includeHistories)
            {
                query = query.Include(a => a.Histories)
                            .ThenInclude(h => h.Question)
                            .Include(a => a.Histories)
                            .ThenInclude(h => h.Answer);
            }

            return await query.FirstOrDefaultAsync(a => a.UserQuizAttemptId == attemptId);
        }

        public async Task<IEnumerable<UserQuizAttempt>> GetByCustomerIdAsync(int customerId, bool includeHistories = false)
        {
            var query = _context.UserQuizAttempts.AsQueryable();

            if (includeHistories)
            {
                query = query.Include(a => a.Histories)
                            .ThenInclude(h => h.Question)
                            .Include(a => a.Histories)
                            .ThenInclude(h => h.Answer);
            }

            return await query.Where(a => a.CustomerId == customerId)
                            .OrderByDescending(a => a.CreatedAt)
                            .ToListAsync();
        }

        public async Task<IEnumerable<UserQuizAttempt>> GetByQuizAndCustomerAsync(int quizId, int customerId, bool includeHistories = false)
        {
            var query = _context.UserQuizAttempts.AsQueryable();

            if (includeHistories)
            {
                query = query.Include(a => a.Histories)
                            .ThenInclude(h => h.Question)
                            .Include(a => a.Histories)
                            .ThenInclude(h => h.Answer);
            }

            return await query.Where(a => a.QuizId == quizId && a.CustomerId == customerId)
                            .OrderByDescending(a => a.CreatedAt)
                            .ToListAsync();
        }

        public async Task<UserQuizAttempt> UpdateAsync(UserQuizAttempt userQuizAttempt)
        {
            _context.Entry(userQuizAttempt).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return userQuizAttempt;
        }

        public async Task<int> GetAttemptNumberAsync(int quizId, int customerId)
        {
            var maxAttemptNumber = await _context.UserQuizAttempts
                .Where(a => a.QuizId == quizId && a.CustomerId == customerId)
                .MaxAsync(a => (int?)a.AttemptNumber) ?? 0;

            return maxAttemptNumber + 1;
        }

        public async Task<bool> ExistsAsync(int attemptId)
        {
            return await _context.UserQuizAttempts.AnyAsync(a => a.UserQuizAttemptId == attemptId);
        }
    }
}
