using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using System.Threading.Tasks;

namespace SWP391_CareSkin_BE.Repositories.Implementations
{
    public class ProductDetailIngredientRepository : IProductDetailIngredientRepository
    {
        private readonly MyDbContext _context;

        public ProductDetailIngredientRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<ProductDetailIngredient> GetByIdAsync(int id)
        {
            return await _context.ProductDetailIngredients.FirstOrDefaultAsync(pdi => pdi.ProductDetailIngredientId == id);
        }

        public async Task DeleteAsync(ProductDetailIngredient productDetailIngredient)
        {
            _context.ProductDetailIngredients.Remove(productDetailIngredient);
            await _context.SaveChangesAsync();
        }
    }
}
