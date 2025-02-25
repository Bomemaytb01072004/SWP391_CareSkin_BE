using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;

namespace SWP391_CareSkin_BE.Repositories.Implementations
{
    public class BrandRepository : IBrandRepository
    {
        private readonly MyDbContext _context;

        public BrandRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<List<Brand>> GetAllBrandsAsync()
        {
            return await _context.Brands.ToListAsync();
        }

        public async Task<Brand> GetBrandByIdAsync(int brandId)
        {
            return await _context.Brands.FirstOrDefaultAsync(b => b.BrandId == brandId);
        }

        public async Task AddBrandAsync(Brand brand)
        {
            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBrandAsync(Brand brand)
        {
            _context.Brands.Update(brand);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBrandAsync(int brandId)
        {
            var brand = await GetBrandByIdAsync(brandId);
            if (brand != null)
            {
                _context.Brands.Remove(brand);
                await _context.SaveChangesAsync();
            }
        }
    }
}
