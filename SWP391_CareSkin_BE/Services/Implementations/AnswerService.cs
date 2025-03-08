using SWP391_CareSkin_BE.DTOs;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Services.Implementations
{
    public class AnswerService : IAnswerService
    {
        private readonly IAnswerRepository _answerRepository;

        public AnswerService(IAnswerRepository answerRepository)
        {
            _answerRepository = answerRepository;
        }

        public async Task<IEnumerable<AnswerDTO>> GetAllAnswersAsync()
        {
            return await _answerRepository.GetAllAsync();
        }

        public async Task<AnswerDTO> GetAnswerByIdAsync(int id)
        {
            return await _answerRepository.GetByIdAsync(id);
        }

        public async Task<AnswerDTO> CreateAnswerAsync(AnswerDTO answerDto)
        {
            return await _answerRepository.AddAsync(answerDto);
        }

        public async Task<AnswerDTO> UpdateAnswerAsync(AnswerDTO answerDto)
        {
            return await _answerRepository.UpdateAsync( answerDto);
        }

        public async Task<bool> DeleteAnswerAsync(int id)
        {
            return await _answerRepository.DeleteAsync(id);
        }

        public async Task<AnswerDTO> UpdateAnswerAsync(int id, AnswerDTO answerDto)
        {
            var existingAnswer = await _answerRepository.GetByIdAsync(id);
            if (existingAnswer == null)
                return null;

            existingAnswer.AnswersContext = answerDto.AnswersContext;
            existingAnswer.PointForSkinType = answerDto.PointForSkinType;
            existingAnswer.QuestionId = answerDto.QuestionId;

            await _answerRepository.UpdateAsync(answerDto);

            return new AnswerDTO
            {
                AnswerId = existingAnswer.AnswerId,
                AnswersContext = existingAnswer.AnswersContext,
                PointForSkinType = existingAnswer.PointForSkinType,
                QuestionId = existingAnswer.QuestionId
            };
        }

    }
}
