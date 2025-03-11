using SWP391_CareSkin_BE.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391_CareSkin_BE.Repositories.Interfaces
{
    public interface IResultRepository
    {
        Task<Result> CreateAsync(Result result);
        Task<Result> UpdateAsync(Result result);
        Task<Result> GetByIdAsync(int resultId);
        Task<IEnumerable<Result>> GetByCustomerIdAsync(int customerId);
        Task<Result> GetLatestByQuizAndCustomerAsync(int quizId, int customerId);
        Task<bool> ExistsAsync(int resultId);
    }
}
