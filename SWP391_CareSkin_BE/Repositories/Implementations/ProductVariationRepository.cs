using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using System.Threading.Tasks;

namespace SWP391_CareSkin_BE.Repositories.Implementations
{
    public class ProductVariationRepository : IProductVariationRepository
    {
        private readonly MyDbContext _context;

        public ProductVariationRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<ProductVariation> GetByIdAsync(int id)
        {
            return await _context.ProductVariations.FirstOrDefaultAsync(pv => pv.ProductVariationId == id);
        }

        public async Task DeleteAsync(ProductVariation productVariation)
        {
            _context.ProductVariations.Remove(productVariation);
            await _context.SaveChangesAsync();
        }
    }
}
