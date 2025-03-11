using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP391_CareSkin_BE.Repositories.Implementations
{
    public class HistoryRepository : IHistoryRepository
    {
        private readonly MyDbContext _context;

        public HistoryRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<History> CreateHistory(History history)
        {
            _context.Histories.Add(history);
            await _context.SaveChangesAsync();
            return history;
        }

        public async Task<History> DeleteHistory(int historyId)
        {
            var history = await _context.Histories.FindAsync(historyId);
            if (history != null)
            {
                _context.Histories.Remove(history);
                await _context.SaveChangesAsync();
            }
            return history;
        }

        public async Task<IEnumerable<History>> GetHistoriesByAttemptId(int attemptId)
        {
            return await _context.Histories
                .Include(h => h.Question)
                .Include(h => h.Answer)
                .Where(h => h.AttemmptId == attemptId)
                .OrderBy(h => h.Question.QuestionsId)
                .ToListAsync();
        }

        public async Task<History> GetHistoryByAttemptAndQuestion(int attemptId, int questionId)
        {
            return await _context.Histories
                .FirstOrDefaultAsync(h => h.AttemmptId == attemptId && h.QuestionId == questionId);
        }

        public async Task<History> GetHistoryById(int historyId)
        {
            return await _context.Histories
                .Include(h => h.Question)
                .Include(h => h.Answer)
                .Include(h => h.UserQuizAttempt)
                .FirstOrDefaultAsync(h => h.HistoryId == historyId);
        }

        public async Task<IEnumerable<History>> GetHistoryByUserId(int userId)
        {
            return await _context.Histories
                .Include(h => h.Question)
                .Include(h => h.Answer)
                .Include(h => h.UserQuizAttempt)
                .Where(h => h.UserQuizAttempt.CustomerId == userId)
                .OrderByDescending(h => h.HistoryId)
                .ToListAsync();
        }

        public async Task<History> UpdateHistory(History history)
        {
            _context.Entry(history).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return history;
        }
    }
}
