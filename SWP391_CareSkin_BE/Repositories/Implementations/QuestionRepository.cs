using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using System;

namespace SWP391_CareSkin_BE.Repositories.Implementations
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly MyDbContext _context;

        public QuestionRepository(MyDbContext context)
        {
            _context = context;
        }

        public List<Question> GetAllQuestions() => _context.Questions.ToList();

        public Question GetQuestionById(int questionId) => _context.Questions.Find(questionId);

        public void AddQuestion(Question question)
        {
            _context.Questions.Add(question);
            SaveChanges();
        }

        public void UpdateQuestion(Question question)
        {
            _context.Questions.Update(question);
            SaveChanges();
        }

        public void DeleteQuestion(int questionId)
        {
            var question = GetQuestionById(questionId);
            if (question != null)
            {
                _context.Questions.Remove(question);
                SaveChanges();
            }
        }
        public void SaveChanges() => _context.SaveChanges();
    }
}
