using SWP391_CareSkin_BE.DTOs;
using SWP391_CareSkin_BE.DTOs.Requests.Question;
using SWP391_CareSkin_BE.DTOs.Responses.Question;
using SWP391_CareSkin_BE.Mappers;
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

        public List<QuestionResponseDTO> GetAllQuestions()
        {
            var questions = _questionRepository.GetAllQuestions();
            return questions.Select(QuestionMapper.ToDTO).ToList();
        }

        public QuestionResponseDTO GetQuestionById(int questionId)
        {
            var question = _questionRepository.GetQuestionById(questionId);
            return question != null ? QuestionMapper.ToDTO(question) : null;
        }

        public void CreateQuestion(CreateQuestionRequestDTO dto)
        {
            var question = QuestionMapper.ToEntity(dto);
            _questionRepository.AddQuestion(question);
        }

        public void UpdateQuestion(UpdateQuestionRequestDTO dto)
        {
            var question = _questionRepository.GetQuestionById(dto.QuestionsId);
            if (question != null)
            {
                question.QuestionContext = dto.QuestionContext;
                _questionRepository.UpdateQuestion(question);
            }
        }

        public void DeleteQuestion(int questionId)
        {
            _questionRepository.DeleteQuestion(questionId);
        }
    }

}
