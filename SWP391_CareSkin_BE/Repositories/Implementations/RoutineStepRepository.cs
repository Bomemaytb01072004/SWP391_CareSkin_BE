using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SWP391_CareSkin_BE.Repositories.Implementations
{
    public class RoutineStepRepository : IRoutineStepRepository
    {
        private readonly MyDbContext _context;

        public RoutineStepRepository(MyDbContext context)
        {
            _context = context;
        }

        // Lấy tất cả các RoutineSteps
        public async Task<List<RoutineStep>> GetAllAsync()
        {
            return await _context.RoutineSteps
                .Include(rs => rs.Routine) // Kết nối với Routine
                .Include(rs => rs.RoutineProducts) // Kết nối với RoutineProduct
                    .ThenInclude(rp => rp.Product) // Kết nối với Product trong RoutineProduct
                .OrderBy(rs => rs.RoutineId) // Sắp xếp theo RoutineId
                .ThenBy(rs => rs.StepOrder) // Sắp xếp theo StepOrder
                .ToListAsync(); // Chạy truy vấn bất đồng bộ
        }

        // Lấy RoutineStep theo ID
        public async Task<RoutineStep> GetByIdAsync(int id)
        {
            return await _context.RoutineSteps
                .Include(rs => rs.Routine) // Kết nối với Routine
                .Include(rs => rs.RoutineProducts) // Kết nối với RoutineProduct
                    .ThenInclude(rp => rp.Product) // Kết nối với Product
                .FirstOrDefaultAsync(rs => rs.RoutineStepId == id); // Lấy theo RoutineStepId
        }

        // Lấy tất cả các RoutineSteps theo RoutineId
        public async Task<List<RoutineStep>> GetByRoutineIdAsync(int routineId)
        {
            return await _context.RoutineSteps
                .Include(rs => rs.Routine) // Kết nối với Routine
                .Include(rs => rs.RoutineProducts) // Kết nối với RoutineProduct
                    .ThenInclude(rp => rp.Product) // Kết nối với Product trong RoutineProduct
                .Where(rs => rs.RoutineId == routineId) // Lọc theo RoutineId
                .OrderBy(rs => rs.StepOrder) // Sắp xếp theo StepOrder
                .ToListAsync(); // Chạy truy vấn bất đồng bộ
        }

        // Lấy RoutineStep theo RoutineId và StepOrder
        public async Task<RoutineStep> GetByRoutineIdAndOrderAsync(int routineId, int stepOrder)
        {
            return await _context.RoutineSteps
                .FirstOrDefaultAsync(rs => rs.RoutineId == routineId && rs.StepOrder == stepOrder); // Lấy theo RoutineId và StepOrder
        }

        // Tạo mới RoutineStep
        public async Task CreateAsync(RoutineStep routineStep)
        {
            _context.RoutineSteps.Add(routineStep); // Thêm RoutineStep vào DbContext
            await _context.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu
        }

        // Cập nhật RoutineStep
        public async Task UpdateAsync(RoutineStep routineStep)
        {
            _context.RoutineSteps.Update(routineStep); // Cập nhật RoutineStep trong DbContext
            await _context.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu
        }

        // Xóa RoutineStep
        public async Task DeleteAsync(RoutineStep routineStep)
        {
            _context.RoutineSteps.Remove(routineStep); // Xóa RoutineStep khỏi DbContext
            await _context.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu
        }
    }
}
