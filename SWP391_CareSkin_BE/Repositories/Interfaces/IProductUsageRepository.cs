using SWP391_CareSkin_BE.Models;
using System.Threading.Tasks;

namespace SWP391_CareSkin_BE.Repositories.Interfaces
{
    public interface IProductUsageRepository
    {
        Task<ProductUsage> GetByIdAsync(int id);
        Task DeleteAsync(ProductUsage productUsage);
    }
}
