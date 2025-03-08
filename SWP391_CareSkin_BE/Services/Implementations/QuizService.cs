using SWP391_CareSkin_BE.DTOs;
using SWP391_CareSkin_BE.DTOs.Requests.Quiz;
using SWP391_CareSkin_BE.DTOs.Responses.Quiz;
using SWP391_CareSkin_BE.Mappers;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Services.Implementations
{
    public class QuizService : IQuizService
    {
        private readonly IQuizRepository _quizRepository;

        public QuizService(IQuizRepository quizRepository)
        {
            _quizRepository = quizRepository;
        }

        public List<QuizResponseDTO> GetAllQuizzes()
        {
            var quizzes = _quizRepository.GetAllQuizzes();
            return quizzes.Select(QuizMapper.ToDTO).ToList();
        }

        public QuizResponseDTO GetQuizById(int quizId)
        {
            var quiz = _quizRepository.GetQuizById(quizId);
            return quiz != null ? QuizMapper.ToDTO(quiz) : null;
        }

        public void CreateQuiz(CreateQuizRequestDTO dto)
        {
            var quiz = QuizMapper.ToEntity(dto);
            _quizRepository.AddQuiz(quiz);
        }

        public void UpdateQuiz(UpdateQuizRequestDTO dto)
        {
            var quiz = _quizRepository.GetQuizById(dto.QuizId);
            if (quiz != null)
            {
                quiz.Title = dto.Title;
                quiz.Description = dto.Description;
                _quizRepository.UpdateQuiz(quiz);
            }
        }

        public void DeleteQuiz(int quizId)
        {
            _quizRepository.DeleteQuiz(quizId);
        }
    }

}
