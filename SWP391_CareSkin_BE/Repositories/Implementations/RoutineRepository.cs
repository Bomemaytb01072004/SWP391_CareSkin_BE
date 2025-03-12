using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace SWP391_CareSkin_BE.Repositories.Implementations
{
    public class RoutineRepository : IRoutineRepository
    {
        private readonly MyDbContext _context;

        public RoutineRepository(MyDbContext context)
        {
            _context = context;
        }

        // Lấy tất cả các Routine
        public async Task<List<Routine>> GetAllAsync()
        {
            return await _context.Routines
                .Include(r => r.SkinType) // Kết nối với SkinType
                .Include(r => r.RoutineSteps) // Kết nối với RoutineSteps
                    .ThenInclude(rs => rs.RoutineProducts) // Kết nối với RoutineProducts của RoutineSteps
                        .ThenInclude(rp => rp.Product) // Kết nối với Product trong RoutineProduct
                            .ThenInclude(p => p.ProductVariations) // Kết nối với ProductVariations trong Product
                .ToListAsync();
        }

        // Lấy Routine theo ID
        public async Task<Routine> GetByIdAsync(int id)
        {
            return await _context.Routines
                .Include(r => r.SkinType) // Kết nối với SkinType
                .Include(r => r.RoutineSteps) // Kết nối với RoutineSteps
                    .ThenInclude(rs => rs.RoutineProducts) // Kết nối với RoutineProducts
                        .ThenInclude(rp => rp.Product) // Kết nối với Product
                            .ThenInclude(p => p.ProductVariations) // Kết nối với ProductVariations
                .Include(r => r.RoutineSteps)
                    .ThenInclude(rs => rs.RoutineProducts) // Kết nối lại RoutineProducts cho đúng
                        .ThenInclude(rp => rp.Product)
                            .ThenInclude(p => p.Brand) // Kết nối với Brand của Product
                .FirstOrDefaultAsync(r => r.RoutineId == id); // Truy vấn theo RoutineId
        }

        // Lấy Routine theo SkinType và Period
        public async Task<List<Routine>> GetBySkinTypeAndPeriodAsync(int skinTypeId, string period)
        {
            return await _context.Routines
                .Where(r => r.SkinTypeId == skinTypeId && r.RoutinePeriod.ToLower() == period.ToLower()) // Lọc theo SkinTypeId và Period
                .Include(r => r.SkinType) // Kết nối với SkinType
                .Include(r => r.RoutineSteps) // Kết nối với RoutineSteps
                    .ThenInclude(rs => rs.RoutineProducts) // Kết nối với RoutineProducts
                        .ThenInclude(rp => rp.Product) // Kết nối với Product trong RoutineProduct
                            .ThenInclude(p => p.ProductVariations) // Kết nối với ProductVariations trong Product
                .Include(r => r.RoutineSteps)
                    .ThenInclude(rs => rs.RoutineProducts)
                        .ThenInclude(rp => rp.Product)
                            .ThenInclude(p => p.Brand) // Kết nối với Brand của Product
                .ToListAsync();
        }

        // Tạo mới Routine
        public async Task CreateAsync(Routine routine)
        {
            _context.Routines.Add(routine); // Thêm Routine vào DbContext
            await _context.SaveChangesAsync(); // Lưu thay đổi
        }

        // Cập nhật Routine
        public async Task UpdateAsync(Routine routine)
        {
            _context.Routines.Update(routine); // Cập nhật Routine
            await _context.SaveChangesAsync(); // Lưu thay đổi
        }

        // Xóa Routine
        public async Task DeleteAsync(Routine routine)
        {
            _context.Routines.Remove(routine); // Xóa Routine
            await _context.SaveChangesAsync(); // Lưu thay đổi
        }
    }
}
