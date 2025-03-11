using SWP391_CareSkin_BE.DTOS.Requests.Result;
using SWP391_CareSkin_BE.DTOS.Responses.Result;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface IResultService
    {
        Task<ResultDTO> CreateResultAsync(CreateResultDTO createResultDTO);
        Task<ResultDTO> UpdateResultScoreAsync(int resultId, int additionalScore);
        Task<ResultDTO> GetResultByIdAsync(int resultId);
        Task<List<ResultDTO>> GetResultsByCustomerIdAsync(int customerId);
        Task<ResultDTO> GetLatestResultByQuizAndCustomerAsync(int quizId, int customerId);
    }
}
