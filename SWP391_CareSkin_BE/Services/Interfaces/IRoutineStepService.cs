using SWP391_CareSkin_BE.DTOS.Requests.Routine;
using SWP391_CareSkin_BE.DTOS.Responses.Routine;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface IRoutineStepService
    {
        Task<List<RoutineStepDTO>> GetAllRoutineStepsAsync();
        Task<RoutineStepDTO> GetRoutineStepByIdAsync(int id);
        Task<List<RoutineStepDTO>> GetRoutineStepsByRoutineIdAsync(int routineId);
        Task<RoutineStepDTO> CreateRoutineStepAsync(RoutineStepCreateRequestDTO request);
        Task<RoutineStepDTO> UpdateRoutineStepAsync(int id, RoutineStepUpdateRequestDTO request);
        Task DeleteRoutineStepAsync(int id);
    }
}
