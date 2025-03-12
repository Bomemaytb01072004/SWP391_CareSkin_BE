using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;

namespace SWP391_CareSkin_BE.Repositories.Implementations
{
    public class RoutineProductRepository : IRoutineProductRepository
    {
        private readonly MyDbContext _context;

        public RoutineProductRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<List<RoutineProduct>> GetAllAsync()
        {
            return await _context.RoutineProducts
                .Include(rp => rp.RoutineStep)
                    .ThenInclude(rs => rs.Routine)
                .Include(rp => rp.Product)
                    .ThenInclude(p => p.ProductVariations)
                .Include(rp => rp.Product)
                    .ThenInclude(p => p.Brand)
                .ToListAsync();
        }

        public async Task<RoutineProduct> GetByIdAsync(int id)
        {
            return await _context.RoutineProducts
                .Include(rp => rp.RoutineStep)
                    .ThenInclude(rs => rs.Routine)
                .Include(rp => rp.Product)
                    .ThenInclude(p => p.ProductVariations)
                .Include(rp => rp.Product)
                    .ThenInclude(p => p.Brand)
                .FirstOrDefaultAsync(rp => rp.RoutineProductId == id);
        }

        public async Task<List<RoutineProduct>> GetByRoutineIdAsync(int routineId)
        {
            return await _context.RoutineProducts
                .Where(rp => rp.RoutineId == routineId)
                .Include(rp => rp.Product)
                    .ThenInclude(p => p.ProductVariations)
                .Include(rp => rp.Product)
                    .ThenInclude(p => p.Brand)
                .ToListAsync();
        }

        public async Task<RoutineProduct> GetByRoutineIdAndProductIdAsync(int routineId, int productId)
        {
            return await _context.RoutineProducts
                .Where(rp => rp.RoutineId == routineId && rp.ProductId == productId)
                .Include(rp => rp.Product)
                    .ThenInclude(p => p.ProductVariations)
                .Include(rp => rp.Product)
                    .ThenInclude(p => p.Brand)
                .FirstOrDefaultAsync();
        }

        public async Task CreateAsync(RoutineProduct routineProduct)
        {
            await _context.RoutineProducts.AddAsync(routineProduct);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(RoutineProduct routineProduct)
        {
            _context.RoutineProducts.Update(routineProduct);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(RoutineProduct routineProduct)
        {
            _context.RoutineProducts.Remove(routineProduct);
            await _context.SaveChangesAsync();
        }
    }
}
