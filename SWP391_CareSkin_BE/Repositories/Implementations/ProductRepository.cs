using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;

namespace SWP391_CareSkin_BE.Repositories.Implementations
{
    public class ProductRepository : IProductRepository
    {
        private readonly MyDbContext _context;
        public ProductRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.ProductVariations)
                .Include(p => p.ProductMainIngredients)
                .Include(p => p.ProductDetailIngredients)
                .Include(p => p.ProductUsages)
                .Include(p => p.PromotionProducts.Where(pp => pp.Promotion.IsActive))
                    .ThenInclude(pp => pp.Promotion)
                .Include(p => p.ProductForSkinTypes)
                    .ThenInclude(ps => ps.SkinType)
                .Include(p => p.ProductPictures)
                .ToListAsync();
        }

        public async Task<List<Product>> GetActiveProductsAsync()
        {
            return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.ProductVariations)
                .Include(p => p.ProductMainIngredients)
                .Include(p => p.ProductDetailIngredients)
                .Include(p => p.ProductUsages)
                .Include(p => p.PromotionProducts.Where(pp => pp.Promotion.IsActive))
                    .ThenInclude(pp => pp.Promotion)
                .Include(p => p.ProductForSkinTypes)
                    .ThenInclude(ps => ps.SkinType)
                .Include(p => p.ProductPictures)
                .Where(p => p.IsActive)
                .ToListAsync();
        }

        public async Task<List<Product>> GetInactiveProductsAsync()
        {
            return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.ProductVariations)
                .Include(p => p.ProductMainIngredients)
                .Include(p => p.ProductDetailIngredients)
                .Include(p => p.ProductUsages)
                .Include(p => p.PromotionProducts.Where(pp => pp.Promotion.IsActive))
                    .ThenInclude(pp => pp.Promotion)
                .Include(p => p.ProductForSkinTypes)
                    .ThenInclude(ps => ps.SkinType)
                .Include(p => p.ProductPictures)
                .Where(p => !p.IsActive)
                .ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.ProductVariations)
                .Include(p => p.ProductMainIngredients)
                .Include(p => p.ProductDetailIngredients)
                .Include(p => p.ProductUsages)
                .Include(p => p.PromotionProducts.Where(pp => pp.Promotion.IsActive))
                    .ThenInclude(pp => pp.Promotion)
                .Include(p => p.ProductForSkinTypes)
                    .ThenInclude(ps => ps.SkinType)
                .Include(p => p.ProductPictures)
                .FirstOrDefaultAsync(p => p.ProductId == productId);
        }

        public async Task<bool> ExistsByNameAsync(string productName)
        {
            return await _context.Products
                .AnyAsync(p => p.ProductName.ToLower() == productName.ToLower());
        }

        public async Task<Product> GetProductByNameAsync(string productName)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.ProductName.ToLower() == productName.ToLower());
        }

        public async Task AddProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public IQueryable<Product> GetQueryable()
        {
            return _context.Products
                .Include(p => p.ProductForSkinTypes)
                    .ThenInclude(ps => ps.SkinType)
                .AsQueryable();
        }
    }
}
