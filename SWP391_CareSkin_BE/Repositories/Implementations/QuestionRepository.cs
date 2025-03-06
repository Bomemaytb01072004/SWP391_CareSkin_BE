using Google;
using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;

namespace SWP391_CareSkin_BE.Repositories.Implementations
{
    public class QuestionRepository : IQuestionRepository

    {
        private readonly MyDbContext _context;

        public QuestionRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<Question> GetByIdAsync(int id)
        {
            return await _context.Questions.Include(q => q.Answers)
                                           .FirstOrDefaultAsync(q => q.QuestionsId == id);
        }

        public async Task<List<Question>> GetAllAsync()
        {
            return await _context.Questions.Include(q => q.Answers).ToListAsync();
        }

        public async Task<Question> AddAsync(Question question)
        {
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();
            return question;
        }

        public async Task<Question> UpdateAsync(Question question)
        {
            _context.Questions.Update(question);
            await _context.SaveChangesAsync();
            return question;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question == null) return false;
            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
