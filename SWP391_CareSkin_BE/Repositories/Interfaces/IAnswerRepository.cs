namespace SWP391_CareSkin_BE.Repositories.Interfaces
{
    public interface IAnswerRepository
    {
        Task<IEnumerable<Models.Answer>> GetByQuestionIdAsync(int questionId);
        Task<Models.Answer> GetByIdAsync(int answerId);
        Task<Models.Answer> CreateAsync(Models.Answer answer);
        Task<Models.Answer> UpdateAsync(Models.Answer answer);
        Task DeleteAsync(int answerId);
        Task<bool> ExistsAsync(int answerId);
    }
}
