using SWP391_CareSkin_BE.DTOS.Requests.Routine;
using SWP391_CareSkin_BE.DTOS.Responses.Routine;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface IRoutineService
    {
        Task<List<RoutineDTO>> GetAllRoutinesAsync();
        Task<List<RoutineDTO>> GetActiveRoutinesAsync();
        Task<List<RoutineDTO>> GetInactiveRoutinesAsync();
        Task<RoutineDTO> GetRoutineByIdAsync(int id);
        Task<List<RoutineDTO>> GetRoutinesBySkinTypeIdAsync(int skinTypeId);
        Task<RoutineDTO> CreateRoutineAsync(RoutineCreateRequestDTO request);
        Task<RoutineDTO> UpdateRoutineAsync(int id, RoutineUpdateRequestDTO request);
        Task DeleteRoutineAsync(int id);
    }
}
