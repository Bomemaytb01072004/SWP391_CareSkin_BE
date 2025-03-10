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

        public async Task<IEnumerable<Question>> GetByQuizIdAsync(int quizId)
        {
            IQueryable<Question> query = _context.Questions.Where(q => q.QuizId == quizId);
            return await query.ToListAsync();
        }

        public async Task<Question> GetByIdAsync(int questionId, bool includeAnswers = false)
        {
            IQueryable<Question> query = _context.Questions;

            if (includeAnswers)
            {
                query = query.Include(q => q.Answers);
            }

            return await query.FirstOrDefaultAsync(q => q.QuestionsId == questionId);
        }

        public async Task<Question> CreateAsync(Question question)
        {
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();
            return question;
        }

        public async Task<Question> UpdateAsync(Question question)
        {
            _context.Entry(question).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return question;
        }

        public async Task DeleteAsync(int questionId)
        {
            var question = await _context.Questions.FindAsync(questionId);
            if (question != null)
            {
                _context.Questions.Remove(question);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int questionId)
        {
            return await _context.Questions.AnyAsync(q => q.QuestionsId == questionId);
        }
    }
}
