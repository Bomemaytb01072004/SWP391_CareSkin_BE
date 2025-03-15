using SWP391_CareSkin_BE.DTOs.Responses.Quiz;
using SWP391_CareSkin_BE.DTOS.Requests.Quiz;
using SWP391_CareSkin_BE.DTOS.Responses.Quiz;
using SWP391_CareSkin_BE.Mappers;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using SWP391_CareSkin_BE.Services.Interfaces;
using System.Threading.Tasks;

namespace SWP391_CareSkin_BE.Services.Implementations
{
    public class QuizService : IQuizService
    {
        private readonly IQuizRepository _quizRepository;

        public QuizService(IQuizRepository quizRepository)
        {
            _quizRepository = quizRepository;
        }

        public async Task<IEnumerable<QuizDTO>> GetAllQuizzesAsync()
        {
            var quizzes = await _quizRepository.GetAllAsync();
            return QuizMapper.ToDTOList(quizzes);
        }

        public async Task<List<QuizDTO>> GetActiveQuizzesAsync()
        {
            var quizzes = await _quizRepository.GetActiveAsync();
            return QuizMapper.ToDTOList(quizzes);
        }

        public async Task<List<QuizDTO>> GetInactiveQuizzesAsync()
        {
            var quizzes = await _quizRepository.GetInactiveAsync();
            return QuizMapper.ToDTOList(quizzes);
        }

        public async Task<QuizDetailsDTO> GetQuizByIdAsync(int quizId)
        {
            var quiz = await _quizRepository.GetByIdAsync(quizId);
            if (quiz == null)
            {
                throw new ArgumentException($"Quiz with ID {quizId} not found");
            }
            
            return QuizMapper.ToDetailsDTO(quiz);
        }

        public async Task<QuizDTO> CreateQuizAsync(CreateQuizDTO createQuizDTO)
        {
            // Kiểm tra xem Quiz với tiêu đề này đã tồn tại và đang active chưa
            var existingQuiz = await _quizRepository.GetByTitleAsync(createQuizDTO.Title);
            
            if (existingQuiz != null && existingQuiz.IsActive)
            {
                throw new ArgumentException($"Bài kiểm tra với tiêu đề '{createQuizDTO.Title}' đã tồn tại.");
            }
            
            var quiz = QuizMapper.ToEntity(createQuizDTO);
            var createdQuiz = await _quizRepository.CreateAsync(quiz);
            
            return QuizMapper.ToDTO(createdQuiz);
        }

        public async Task<QuizDTO> UpdateQuizAsync(int quizId, UpdateQuizDTO updateQuizDTO)
        {
            var existingQuiz = await _quizRepository.GetByIdAsync(quizId);
            if (existingQuiz == null)
            {
                throw new ArgumentException($"Quiz with ID {quizId} not found");
            }
            
            QuizMapper.UpdateEntity(existingQuiz, updateQuizDTO);
            var updatedQuiz = await _quizRepository.UpdateAsync(existingQuiz);
            
            return QuizMapper.ToDTO(updatedQuiz);
        }

        public async Task DeleteQuizAsync(int quizId)
        {
            var quiz = await _quizRepository.GetByIdAsync(quizId);
            if (quiz == null)
            {
                throw new ArgumentException($"Quiz with ID {quizId} not found");
            }
            
            // Implement soft delete by setting IsActive to false
            quiz.IsActive = false;
            await _quizRepository.UpdateAsync(quiz);
        }
    }
}
