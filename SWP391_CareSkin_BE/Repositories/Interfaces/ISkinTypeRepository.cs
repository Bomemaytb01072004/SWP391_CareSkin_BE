using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Repositories.Interfaces
{
    public interface ISkinTypeRepository
    {
        Task<List<SkinType>> GetAllAsync();
        Task<SkinType> GetByIdAsync(int id);
        Task<SkinType> CreateAsync(SkinType skinType);
        Task<SkinType> UpdateAsync(SkinType skinType);
        Task DeleteAsync(SkinType skinType);
        Task<bool> ExistsAsync(int id);
    }
}
