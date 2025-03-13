using SWP391_CareSkin_BE.Models;
using System.Threading.Tasks;

namespace SWP391_CareSkin_BE.Repositories.Interfaces
{
    public interface IProductMainIngredientRepository
    {
        Task<ProductMainIngredient> GetByIdAsync(int id);
        Task DeleteAsync(ProductMainIngredient productMainIngredient);
    }
}
