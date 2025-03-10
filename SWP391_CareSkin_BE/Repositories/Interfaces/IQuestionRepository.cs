namespace SWP391_CareSkin_BE.Repositories.Interfaces
{
    public interface IQuestionRepository
    {
        Task<IEnumerable<Models.Question>> GetByQuizIdAsync(int quizId);
        Task<Models.Question> GetByIdAsync(int questionId, bool includeAnswers = false);
        Task<Models.Question> CreateAsync(Models.Question question);
        Task<Models.Question> UpdateAsync(Models.Question question);
        Task DeleteAsync(int questionId);
        Task<bool> ExistsAsync(int questionId);
    }
}
