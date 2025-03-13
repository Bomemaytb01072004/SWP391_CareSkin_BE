using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using System.Threading.Tasks;

namespace SWP391_CareSkin_BE.Repositories.Implementations
{
    public class ProductForSkinTypeRepository : IProductForSkinTypeRepository
    {
        private readonly MyDbContext _context;

        public ProductForSkinTypeRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<ProductForSkinType> GetByIdAsync(int id)
        {
            return await _context.ProductForSkinTypes.FirstOrDefaultAsync(pfst => pfst.ProductForSkinTypeId == id);
        }

        public async Task DeleteAsync(ProductForSkinType productForSkinType)
        {
            _context.ProductForSkinTypes.Remove(productForSkinType);
            await _context.SaveChangesAsync();
        }
    }
}
