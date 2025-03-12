using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;

namespace SWP391_CareSkin_BE.Repositories.Implementations
{
    public class RoutineStepRepository : IRoutineStepRepository
    {
        private readonly MyDbContext _context;

        public RoutineStepRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<List<RoutineStep>> GetAllAsync()
        {
            return await _context.RoutineSteps
                .Include(rs => rs.Routine)
                .Include(rs => rs.RoutineProduct)
                    .ThenInclude(rp => rp.Product)
                .OrderBy(rs => rs.RoutineId)
                .ThenBy(rs => rs.StepOrder)
                .ToListAsync();
        }

        public async Task<RoutineStep> GetByIdAsync(int id)
        {
            return await _context.RoutineSteps
                .Include(rs => rs.Routine)
                .Include(rs => rs.RoutineProduct)
                    .ThenInclude(rp => rp.Product)
                .FirstOrDefaultAsync(rs => rs.RoutineStepId == id);
        }

        public async Task<List<RoutineStep>> GetByRoutineIdAsync(int routineId)
        {
            return await _context.RoutineSteps
                .Include(rs => rs.Routine)
                .Include(rs => rs.RoutineProduct)
                    .ThenInclude(rp => rp.Product)
                .Where(rs => rs.RoutineId == routineId)
                .OrderBy(rs => rs.StepOrder)
                .ToListAsync();
        }

        public async Task<RoutineStep> GetByRoutineIdAndOrderAsync(int routineId, int stepOrder)
        {
            return await _context.RoutineSteps
                .FirstOrDefaultAsync(rs => rs.RoutineId == routineId && rs.StepOrder == stepOrder);
        }

        public async Task CreateAsync(RoutineStep routineStep)
        {
            _context.RoutineSteps.Add(routineStep);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(RoutineStep routineStep)
        {
            _context.RoutineSteps.Update(routineStep);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(RoutineStep routineStep)
        {
            _context.RoutineSteps.Remove(routineStep);
            await _context.SaveChangesAsync();
        }
    }
}
