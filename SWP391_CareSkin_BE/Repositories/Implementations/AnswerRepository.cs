using Google;
using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;

namespace SWP391_CareSkin_BE.Repositories.Implementations
{
    public class AnswerRepository : IAnswerRepository
    {
        private readonly MyDbContext _context;

        public AnswerRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<List<Answer>> GetAllAsync()
        {
            return await _context.Answers.ToListAsync();
        }

        public async Task<Answer> GetByIdAsync(int id)
        {
            return await _context.Answers.FindAsync(id);
        }

        public async Task<Answer> AddAsync(Answer answer)
        {
            _context.Answers.Add(answer);
            await _context.SaveChangesAsync();
            return answer;
        }

        public async Task<Answer> UpdateAsync(Answer answer)
        {
            _context.Answers.Update(answer);
            await _context.SaveChangesAsync();
            return answer;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var answer = await _context.Answers.FindAsync(id);
            if (answer == null) return false;
            _context.Answers.Remove(answer);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
