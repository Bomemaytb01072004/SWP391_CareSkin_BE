using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Repositories.Interfaces
{
    public interface IRoutineStepRepository
    {
        Task<List<RoutineStep>> GetAllAsync();
        Task<RoutineStep> GetByIdAsync(int id);
        Task<List<RoutineStep>> GetByRoutineIdAsync(int routineId);
        Task<RoutineStep> GetByRoutineIdAndOrderAsync(int routineId, int stepOrder);
        Task CreateAsync(RoutineStep routineStep);
        Task UpdateAsync(RoutineStep routineStep);
        Task DeleteAsync(RoutineStep routineStep);
    }
}
