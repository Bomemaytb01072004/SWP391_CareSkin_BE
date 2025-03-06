using SWP391_CareSkin_BE.DTOs;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Implementations;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Services.Implementations
{
    public class QuestionService : IQuestionService

    {
        private readonly QuestionRepository _questionRepository;

        public QuestionService(QuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public async Task<List<Question>> GetAllQuestionsAsync()
        {
            return await _questionRepository.GetAllAsync();
        }

        public async Task<Question> GetQuestionByIdAsync(int id)
        {
            return await _questionRepository.GetByIdAsync(id);
        }

        public async Task<Question> CreateQuestionAsync(QuestionDTO dto)
        {
            var question = new Question
            {
                QuizId = dto.QuizId,
                QuestionContext = dto.QuestionContext
            };

            return await _questionRepository.AddAsync(question);
        }

        public async Task<Question> UpdateQuestionAsync(int id, QuestionDTO dto)
        {
            var question = await _questionRepository.GetByIdAsync(id);
            if (question == null) return null;

            question.QuestionContext = dto.QuestionContext;
            return await _questionRepository.UpdateAsync(question);
        }

        public async Task<bool> DeleteQuestionAsync(int id)
        {
            return await _questionRepository.DeleteAsync(id);
        }
    }
}
