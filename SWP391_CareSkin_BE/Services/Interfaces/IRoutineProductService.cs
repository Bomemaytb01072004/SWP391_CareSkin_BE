using SWP391_CareSkin_BE.DTOS.Requests.Routine;
using SWP391_CareSkin_BE.DTOS.Responses.Routine;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface IRoutineProductService
    {
        Task<List<RoutineProductDTO>> GetAllRoutineProductsAsync();
        Task<RoutineProductDTO> GetRoutineProductByIdAsync(int id);
        Task<List<RoutineProductDTO>> GetRoutineProductsByRoutineIdAsync(int routineId);
        Task<RoutineProductDTO> CreateRoutineProductAsync(RoutineProductCreateRequestDTO request);
        Task<RoutineProductDTO> UpdateRoutineProductAsync(int id, RoutineProductUpdateRequestDTO request);
        Task DeleteRoutineProductAsync(int id);
    }
}
