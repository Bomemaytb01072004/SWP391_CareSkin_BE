using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using System.Threading.Tasks;

namespace SWP391_CareSkin_BE.Repositories.Implementations
{
    public class ProductUsageRepository : IProductUsageRepository
    {
        private readonly MyDbContext _context;

        public ProductUsageRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<ProductUsage> GetByIdAsync(int id)
        {
            return await _context.ProductUsages.FirstOrDefaultAsync(pu => pu.ProductUsageId == id);
        }

        public async Task DeleteAsync(ProductUsage productUsage)
        {
            _context.ProductUsages.Remove(productUsage);
            await _context.SaveChangesAsync();
        }
    }
}
