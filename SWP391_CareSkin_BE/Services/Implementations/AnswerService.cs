using SWP391_CareSkin_BE.DTOs;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Implementations;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Services.Implementations
{
    public class AnswerService : IAnswerService
    {
        private readonly AnswerRepository _answerRepository;

        public AnswerService(AnswerRepository answerRepository)
        {
            _answerRepository = answerRepository;
        }

        public async Task<List<Answer>> GetAllAnswersAsync()
        {
            return await _answerRepository.GetAllAsync();
        }

        public async Task<Answer> GetAnswerByIdAsync(int id)
        {
            return await _answerRepository.GetByIdAsync(id);
        }

        public async Task<Answer> CreateAnswerAsync(AnswerDTO dto)
        {
            var answer = new Answer
            {
                QuestionId = dto.QuestionId,
                AnswersContext = dto.AnswersContext,
                PointForSkinType = dto.PointForSkinType
            };

            return await _answerRepository.AddAsync(answer);
        }

        public async Task<Answer> UpdateAnswerAsync(int id, AnswerDTO dto)
        {
            var answer = await _answerRepository.GetByIdAsync(id);
            if (answer == null) return null;

            answer.AnswersContext = dto.AnswersContext;
            answer.PointForSkinType = dto.PointForSkinType;
            return await _answerRepository.UpdateAsync(answer);
        }

        public async Task<bool> DeleteAnswerAsync(int id)
        {
            return await _answerRepository.DeleteAsync(id);
        }
    }
}
