using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using System.Threading.Tasks;

namespace SWP391_CareSkin_BE.Repositories.Implementations
{
    public class ProductMainIngredientRepository : IProductMainIngredientRepository
    {
        private readonly MyDbContext _context;

        public ProductMainIngredientRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<ProductMainIngredient> GetByIdAsync(int id)
        {
            return await _context.ProductMainIngredients.FirstOrDefaultAsync(pmi => pmi.ProductMainIngredientId == id);
        }

        public async Task DeleteAsync(ProductMainIngredient productMainIngredient)
        {
            _context.ProductMainIngredients.Remove(productMainIngredient);
            await _context.SaveChangesAsync();
        }
    }
}
