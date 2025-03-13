using SWP391_CareSkin_BE.Models;
using System.Threading.Tasks;

namespace SWP391_CareSkin_BE.Repositories.Interfaces
{
    public interface IProductDetailIngredientRepository
    {
        Task<ProductDetailIngredient> GetByIdAsync(int id);
        Task DeleteAsync(ProductDetailIngredient productDetailIngredient);
    }
}
