using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Repositories.Interfaces
{
    public interface IRoutineRepository
    {
        Task<List<Routine>> GetAllAsync();
        Task<Routine> GetByIdAsync(int id);
        Task<List<Routine>> GetBySkinTypeIdAsync(int skinTypeId);
        Task<List<Routine>> GetBySkinTypeAndPeriodAsync(int skinTypeId, string period);
        Task CreateAsync(Routine routine);
        Task UpdateAsync(Routine routine);
        Task DeleteAsync(Routine routine);
    }
}
