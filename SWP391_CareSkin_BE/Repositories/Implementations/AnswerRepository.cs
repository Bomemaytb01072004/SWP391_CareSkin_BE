using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using System;

namespace SWP391_CareSkin_BE.Repositories.Implementations
{
    public class AnswerRepository : IAnswerRepository
    {
        private readonly MyDbContext _context;

        public AnswerRepository(MyDbContext context)
        {
            _context = context;
        }

        public List<Answer> GetAllAnswers() => _context.Answers.ToList();

        public Answer GetAnswerById(int answerId) => _context.Answers.Find(answerId);

        public void AddAnswer(Answer answer)
        {
            _context.Answers.Add(answer);
            SaveChanges();
        }

        public void UpdateAnswer(Answer answer)
        {
            _context.Answers.Update(answer);
            SaveChanges();
        }

        public void DeleteAnswer(int answerId)
        {
            var answer = GetAnswerById(answerId);
            if (answer != null)
            {
                _context.Answers.Remove(answer);
                SaveChanges();
            }
        }
        public void SaveChanges() => _context.SaveChanges();
    }

}
