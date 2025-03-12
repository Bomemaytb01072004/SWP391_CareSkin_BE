using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;

namespace SWP391_CareSkin_BE.Repositories.Implementations
{
    public class RoutineRepository : IRoutineRepository
    {
        private readonly MyDbContext _context;

        public RoutineRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<List<Routine>> GetAllAsync()
        {
            return await _context.Routines
                .Include(r => r.SkinType)
                .Include(r => r.RoutineProducts)
                    .ThenInclude(rp => rp.Product)
                        .ThenInclude(p => p.ProductVariations)
                .Include(r => r.RoutineProducts)
                    .ThenInclude(rp => rp.Product)
                        .ThenInclude(p => p.Brand)
                .ToListAsync();
        }

        public async Task<Routine> GetByIdAsync(int id)
        {
            return await _context.Routines
                .Include(r => r.SkinType)
                .Include(r => r.RoutineProducts)
                    .ThenInclude(rp => rp.Product)
                        .ThenInclude(p => p.ProductVariations)
                .Include(r => r.RoutineProducts)
                    .ThenInclude(rp => rp.Product)
                        .ThenInclude(p => p.Brand)
                .FirstOrDefaultAsync(r => r.RoutineId == id);
        }

        public async Task<List<Routine>> GetBySkinTypeAndPeriodAsync(int skinTypeId, string period)
        {
            return await _context.Routines
                .Where(r => r.SkinTypeId == skinTypeId && r.RoutinePeriod.ToLower() == period.ToLower())
                .Include(r => r.SkinType)
                .Include(r => r.RoutineProducts)
                    .ThenInclude(rp => rp.Product)
                        .ThenInclude(p => p.ProductVariations)
                .Include(r => r.RoutineProducts)
                    .ThenInclude(rp => rp.Product)
                        .ThenInclude(p => p.Brand)
                .ToListAsync();
        }

        public async Task CreateAsync(Routine routine)
        {
            _context.Routines.Add(routine);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Routine routine)
        {
            _context.Routines.Update(routine);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Routine routine)
        {
            _context.Routines.Remove(routine);
            await _context.SaveChangesAsync();
        }
    }
}
