using SWP391_CareSkin_BE.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391_CareSkin_BE.Repositories.Interfaces
{
    public interface IRoutineProductRepository
    {
        Task<List<RoutineProduct>> GetAllAsync();
        Task<RoutineProduct> GetByIdAsync(int id);
        Task<List<RoutineProduct>> GetByRoutineStepIdAsync(int routineStepId);
        Task<RoutineProduct> GetByStepIdAndProductIdAsync(int stepId, int productId);
        Task CreateAsync(RoutineProduct routineProduct);
        Task UpdateAsync(RoutineProduct routineProduct);
        Task DeleteAsync(RoutineProduct routineProduct);
    }
}
