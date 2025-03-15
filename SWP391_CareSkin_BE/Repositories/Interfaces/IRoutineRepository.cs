using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Repositories.Interfaces
{
    public interface IRoutineRepository
    {
        Task<List<Routine>> GetAllAsync();
        Task<List<Routine>> GetActiveAsync();
        Task<List<Routine>> GetInactiveAsync();
        Task<Routine> GetByIdAsync(int id);
        Task<List<Routine>> GetBySkinTypeIdAsync(int skinTypeId);
        Task<List<Routine>> GetBySkinTypeAndPeriodAsync(int skinTypeId, string period);
        Task CreateAsync(Routine routine);
        Task UpdateAsync(Routine routine);
        Task<Routine> GetByNameAndPeriodAsync(string name, string period, int skinTypeId);
    }
}
