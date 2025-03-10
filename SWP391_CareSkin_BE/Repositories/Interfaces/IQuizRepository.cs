namespace SWP391_CareSkin_BE.Repositories.Interfaces
{
    public interface IQuizRepository
    {
        Task<IEnumerable<Models.Quiz>> GetAllAsync();
        Task<Models.Quiz> GetByIdAsync(int quizId, bool includeQuestions = false);
        Task<Models.Quiz> CreateAsync(Models.Quiz quiz);
        Task<Models.Quiz> UpdateAsync(Models.Quiz quiz);
        Task DeleteAsync(int quizId);
        Task<bool> ExistsAsync(int quizId);
    }
}
