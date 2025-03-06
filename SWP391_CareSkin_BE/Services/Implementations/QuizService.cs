using SWP391_CareSkin_BE.DTOs;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Implementations;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Services.Implementations
{
    public class QuizService : IQuizService
    {
        private readonly QuizRepository _quizRepository;

        public QuizService(QuizRepository quizRepository)
        {
            _quizRepository = quizRepository;
        }

        public async Task<List<Quiz>> GetAllQuizzesAsync()
        {
            return await _quizRepository.GetAllAsync();
        }

        public async Task<Quiz> GetQuizByIdAsync(int id)
        {
            return await _quizRepository.GetByIdAsync(id);
        }

        public async Task<Quiz> CreateQuizAsync(QuizDTO dto)
        {
            var quiz = new Quiz
            {
                Title = dto.Title,
                Description = dto.Description
            };

            return await _quizRepository.AddAsync(quiz);
        }

        public async Task<Quiz> UpdateQuizAsync(int id, QuizDTO dto)
        {
            var quiz = await _quizRepository.GetByIdAsync(id);
            if (quiz == null) return null;

            quiz.Title = dto.Title;
            quiz.Description = dto.Description;
            return await _quizRepository.UpdateAsync(quiz);
        }

        public async Task<bool> DeleteQuizAsync(int id)
        {
            return await _quizRepository.DeleteAsync(id);
        }
    }
}
