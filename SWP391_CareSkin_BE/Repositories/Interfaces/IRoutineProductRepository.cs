using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Repositories.Interfaces
{
    public interface IRoutineProductRepository
    {
        Task<List<RoutineProduct>> GetAllAsync();
        Task<RoutineProduct> GetByIdAsync(int id);
        Task<List<RoutineProduct>> GetByRoutineIdAsync(int routineId);
        Task<RoutineProduct> GetByRoutineIdAndProductIdAsync(int routineId, int productId);
        Task CreateAsync(RoutineProduct routineProduct);
        Task UpdateAsync(RoutineProduct routineProduct);
        Task DeleteAsync(RoutineProduct routineProduct);
    }
}
