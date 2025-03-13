using SWP391_CareSkin_BE.Models;
using System.Threading.Tasks;

namespace SWP391_CareSkin_BE.Repositories.Interfaces
{
    public interface IProductVariationRepository
    {
        Task<ProductVariation> GetByIdAsync(int id);
        Task DeleteAsync(ProductVariation productVariation);
    }
}
