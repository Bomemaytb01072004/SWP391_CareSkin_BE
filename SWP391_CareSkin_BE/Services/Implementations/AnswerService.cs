using SWP391_CareSkin_BE.DTOs;
using SWP391_CareSkin_BE.DTOs.Requests.Answer;
using SWP391_CareSkin_BE.DTOs.Responses.Answer;
using SWP391_CareSkin_BE.Mappers;
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

        public List<AnswerResponseDTO> GetAllAnswers()
        {
            var answers = _answerRepository.GetAllAnswers();
            return answers.Select(AnswerMapper.ToDTO).ToList();
        }

        public AnswerResponseDTO GetAnswerById(int answerId)
        {
            var answer = _answerRepository.GetAnswerById(answerId);
            return answer != null ? AnswerMapper.ToDTO(answer) : null;
        }

        public void CreateAnswer(CreateAnswerRequestDTO dto)
        {
            var answer = AnswerMapper.ToEntity(dto);
            _answerRepository.AddAnswer(answer);
        }

        public void UpdateAnswer(UpdateAnswerRequestDTO dto)
        {
            var answer = _answerRepository.GetAnswerById(dto.AnswerId);
            if (answer != null)
            {
                answer.AnswersContext = dto.AnswersContext;
                answer.PointForSkinType = dto.PointForSkinType;
                _answerRepository.UpdateAnswer(answer);
            }
        }

        public void DeleteAnswer(int answerId)
        {
            _answerRepository.DeleteAnswer(answerId);
        }
    }

}
