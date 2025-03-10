using SWP391_CareSkin_BE.DTOS.Requests.Answer;
using SWP391_CareSkin_BE.DTOS.Responses.Answer;
using SWP391_CareSkin_BE.Mappers;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using SWP391_CareSkin_BE.Services.Interfaces;
using System.Threading.Tasks;

namespace SWP391_CareSkin_BE.Services.Implementations
{
    public class AnswerService : IAnswerService
    {
        private readonly IAnswerRepository _answerRepository;
        private readonly IQuestionRepository _questionRepository;

        public AnswerService(IAnswerRepository answerRepository, IQuestionRepository questionRepository)
        {
            _answerRepository = answerRepository;
            _questionRepository = questionRepository;
        }

        public async Task<List<AnswerDTO>> GetAnswersByQuestionAsync(int questionId)
        {
            var questionExists = await _questionRepository.ExistsAsync(questionId);
            if (!questionExists)
            {
                throw new ArgumentException($"Question with ID {questionId} not found");
            }

            var answers = await _answerRepository.GetByQuestionIdAsync(questionId);
            return AnswerMapper.ToDTOList(answers);
        }

        public async Task<AnswerDTO> GetAnswerByIdAsync(int answerId)
        {
            var answer = await _answerRepository.GetByIdAsync(answerId);
            if (answer == null)
            {
                throw new ArgumentException($"Answer with ID {answerId} not found");
            }

            return AnswerMapper.ToDTO(answer);
        }

        public async Task<AnswerDTO> CreateAnswerAsync(int questionId, CreateAnswerDTO createAnswerDTO)
        {
            var questionExists = await _questionRepository.ExistsAsync(questionId);
            if (!questionExists)
            {
                throw new ArgumentException($"Question with ID {questionId} not found");
            }

            var answer = AnswerMapper.ToEntity(questionId, createAnswerDTO);
            var createdAnswer = await _answerRepository.CreateAsync(answer);

            return AnswerMapper.ToDTO(createdAnswer);
        }

        public async Task<AnswerDTO> UpdateAnswerAsync(int answerId, UpdateAnswerDTO updateAnswerDTO)
        {
            var existingAnswer = await _answerRepository.GetByIdAsync(answerId);
            if (existingAnswer == null)
            {
                throw new ArgumentException($"Answer with ID {answerId} not found");
            }

            AnswerMapper.UpdateEntity(existingAnswer, updateAnswerDTO);
            var updatedAnswer = await _answerRepository.UpdateAsync(existingAnswer);

            return AnswerMapper.ToDTO(updatedAnswer);
        }

        public async Task DeleteAnswerAsync(int answerId)
        {
            var exists = await _answerRepository.ExistsAsync(answerId);
            if (!exists)
            {
                throw new ArgumentException($"Answer with ID {answerId} not found");
            }

            await _answerRepository.DeleteAsync(answerId);
        }
    }
}
