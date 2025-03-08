using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using System;

namespace SWP391_CareSkin_BE.Repositories.Implementations
{
    public class QuizRepository : IQuizRepository
    {
        private readonly MyDbContext _context;

        public QuizRepository(MyDbContext context)
        {
            _context = context;
        }

        public List<Quiz> GetAllQuizzes() => _context.Quizs.ToList();

        public Quiz GetQuizById(int quizId) => _context.Quizs.Find(quizId);

        public void AddQuiz(Quiz quiz)
        {
            _context.Quizs.Add(quiz);
            SaveChanges();
        }

        public void UpdateQuiz(Quiz quiz)
        {
            _context.Quizs.Update(quiz);
            SaveChanges();
        }

        public void DeleteQuiz(int quizId)
        {
            var quiz = GetQuizById(quizId);
            if (quiz != null)
            {
                _context.Quizs.Remove(quiz);
                SaveChanges();
            }
        }
        public void SaveChanges() => _context.SaveChanges();
    }
}
