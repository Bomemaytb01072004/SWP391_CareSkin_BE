using SWP391_CareSkin_BE.DTOs;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Services.Implementations
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;

        public QuestionService(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public async Task<IEnumerable<QuestionDTO>> GetAllQuestionsAsync()
        {
            return await _questionRepository.GetAllAsync();
        }

        public async Task<QuestionDTO> GetQuestionByIdAsync(int id)
        {
            return await _questionRepository.GetByIdAsync(id);
        }

        public async Task<QuestionDTO> CreateQuestionAsync(QuestionDTO questionDto)
        {
            return await _questionRepository.AddAsync(questionDto);
        }

        public async Task<QuestionDTO> UpdateQuestionAsync(QuestionDTO questionDto)
        {
            return await _questionRepository.UpdateAsync(questionDto);
        }

        public async Task<bool> DeleteQuestionAsync(int id)
        {
            return await _questionRepository.DeleteAsync(id);
        }
    }
}
