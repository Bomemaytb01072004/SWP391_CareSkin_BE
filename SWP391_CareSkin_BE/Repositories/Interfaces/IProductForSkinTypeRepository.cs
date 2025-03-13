using SWP391_CareSkin_BE.Models;
using System.Threading.Tasks;

namespace SWP391_CareSkin_BE.Repositories.Interfaces
{
    public interface IProductForSkinTypeRepository
    {
        Task<ProductForSkinType> GetByIdAsync(int id);
        Task DeleteAsync(ProductForSkinType productForSkinType);
    }
}
