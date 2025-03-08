using SWP391_CareSkin_BE.DTOs;
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
        public async Task<QuizDTO> CreateQuizAsync(QuizDTO quizDto)
        {
            return await _quizRepository.AddAsync(quizDto);
        }

        public async Task<bool> DeleteQuizAsync(int id)
        {
            return await _quizRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<QuizDTO>> GetAllQuizzesAsync()
        {
            return await _quizRepository.GetAllAsync();
        }

        public async Task<QuizDTO> GetQuizByIdAsync(int id)
        {
            return await _quizRepository.GetByIdAsync(id);
        }

        public async Task<QuizDTO> UpdateQuizAsync(QuizDTO quizDto)
        {
            return await _quizRepository.UpdateAsync(quizDto);
        }

        public async Task<QuizDTO> UpdateQuizAsync(int id, QuizDTO quizDto)
        {
            var existingQuiz = await _quizRepository.GetByIdAsync(id);
            if (existingQuiz == null)
                return null; 
            existingQuiz.Title = quizDto.Title;
            existingQuiz.Description = quizDto.Description;

            await _quizRepository.UpdateAsync(existingQuiz);

            return new QuizDTO
            {
                QuizId = existingQuiz.QuizId,
                Title = existingQuiz.Title,
                Description = existingQuiz.Description
            };
        }

    }
}
