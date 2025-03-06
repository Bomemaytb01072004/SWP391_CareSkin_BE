using Google;
using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;

namespace SWP391_CareSkin_BE.Repositories.Implementations
{
    public class QuizRepository : IQuizRepository
    {
        private readonly MyDbContext _context;

        public QuizRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<List<Quiz>> GetAllAsync()
        {
            return await _context.Quizs.Include(q => q.Questions).ToListAsync();
        }

        public async Task<Quiz> GetByIdAsync(int id)
        {
            return await _context.Quizs.Include(q => q.Questions)
                                       .FirstOrDefaultAsync(q => q.QuizId == id);
        }

        public async Task<Quiz> AddAsync(Quiz quiz)
        {
            _context.Quizs.Add(quiz);
            await _context.SaveChangesAsync();
            return quiz;
        }

        public async Task<Quiz> UpdateAsync(Quiz quiz)
        {
            _context.Quizs.Update(quiz);
            await _context.SaveChangesAsync();
            return quiz;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var quiz = await _context.Quizs.FindAsync(id);
            if (quiz == null) return false;
            _context.Quizs.Remove(quiz);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
