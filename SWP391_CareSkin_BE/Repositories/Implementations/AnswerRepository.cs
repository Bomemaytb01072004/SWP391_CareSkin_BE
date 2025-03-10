using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using System.Threading.Tasks;

namespace SWP391_CareSkin_BE.Repositories.Implementations
{
    public class AnswerRepository : IAnswerRepository
    {
        private readonly MyDbContext _context;

        public AnswerRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Answer>> GetByQuestionIdAsync(int questionId)
        {
            return await _context.Answers
                .Where(a => a.QuestionId == questionId)
                .ToListAsync();
        }

        public async Task<Answer> GetByIdAsync(int answerId)
        {
            return await _context.Answers
                .FirstOrDefaultAsync(a => a.AnswerId == answerId);
        }

        public async Task<Answer> CreateAsync(Answer answer)
        {
            _context.Answers.Add(answer);
            await _context.SaveChangesAsync();
            return answer;
        }

        public async Task<Answer> UpdateAsync(Answer answer)
        {
            _context.Entry(answer).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return answer;
        }

        public async Task DeleteAsync(int answerId)
        {
            var answer = await _context.Answers.FindAsync(answerId);
            if (answer != null)
            {
                _context.Answers.Remove(answer);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int answerId)
        {
            return await _context.Answers.AnyAsync(a => a.AnswerId == answerId);
        }
    }
}
