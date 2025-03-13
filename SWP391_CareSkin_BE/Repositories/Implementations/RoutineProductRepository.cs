using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                .Include(rp => rp.Product)
                    .ThenInclude(p => p.ProductVariations)
                .Include(rp => rp.Product)
                    .ThenInclude(p => p.Brand)
                .Include(rp => rp.RoutineStep)
                .ToListAsync();
        }

        public async Task<RoutineProduct> GetByIdAsync(int id)
        {
            return await _context.RoutineProducts
                .Include(rp => rp.Product)
                    .ThenInclude(p => p.ProductVariations)
                .Include(rp => rp.Product)
                    .ThenInclude(p => p.Brand)
                .Include(rp => rp.RoutineStep)
                .FirstOrDefaultAsync(rp => rp.RoutineProductId == id);
        }

        public async Task<List<RoutineProduct>> GetByRoutineStepIdAsync(int routineStepId)
        {
            return await _context.RoutineProducts
                .Where(rp => rp.RoutineStepId == routineStepId)
                .Include(rp => rp.Product)
                    .ThenInclude(p => p.ProductVariations)
                .Include(rp => rp.Product)
                    .ThenInclude(p => p.Brand)
                .Include(rp => rp.RoutineStep)
                .ToListAsync();
        }

        public async Task<RoutineProduct> GetByStepIdAndProductIdAsync(int stepId, int productId)
        {
            return await _context.RoutineProducts
                .FirstOrDefaultAsync(rp => rp.RoutineStepId == stepId && rp.ProductId == productId);
        }

        public async Task CreateAsync(RoutineProduct routineProduct)
        {
            _context.RoutineProducts.Add(routineProduct);
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
