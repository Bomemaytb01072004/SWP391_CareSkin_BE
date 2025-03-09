using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP391_CareSkin_BE.Repositories.Implementations
{
    public class ProductPictureRepository : IProductPictureRepository
    {
        private readonly MyDbContext _context;

        public ProductPictureRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductPicture>> GetAllProductPicturesAsync()
        {
            return await _context.ProductPictures
                .Include(pp => pp.Product)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductPicture>> GetProductPicturesByProductIdAsync(int productId)
        {
            return await _context.ProductPictures
                .Include(pp => pp.Product)
                .Where(pp => pp.ProductId == productId)
                .ToListAsync();
        }

        public async Task<ProductPicture> GetProductPictureByIdAsync(int id)
        {
            return await _context.ProductPictures
                .Include(pp => pp.Product)
                .FirstOrDefaultAsync(pp => pp.ProductPictureId == id);
        }

        public async Task CreateProductPictureAsync(ProductPicture productPicture)
        {
            _context.ProductPictures.Add(productPicture);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductPictureAsync(ProductPicture productPicture)
        {
            _context.ProductPictures.Update(productPicture);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteProductPictureAsync(int id)
        {
            var productPicture = await _context.ProductPictures.FindAsync(id);
            if (productPicture == null)
                return false;

            _context.ProductPictures.Remove(productPicture);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProductPicturesByProductIdAsync(int productId)
        {
            var productPictures = await _context.ProductPictures
                .Where(pp => pp.ProductId == productId)
                .ToListAsync();

            if (!productPictures.Any())
                return false;

            _context.ProductPictures.RemoveRange(productPictures);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
