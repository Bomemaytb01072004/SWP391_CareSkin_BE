using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Repositories.Interfaces
{
    public interface IBrandRepository
    {
        Task<List<Brand>> GetAllBrandsAsync();
        Task<List<Brand>> GetActiveBrandsAsync();
        Task<List<Brand>> GetInactiveBrandsAsync();
        Task<Brand> GetBrandByIdAsync(int brandId);
        Task AddBrandAsync(Brand brand);
        Task UpdateBrandAsync(Brand brand);
        Task<Brand> GetBrandByNameAsync(string name);
    }
}
