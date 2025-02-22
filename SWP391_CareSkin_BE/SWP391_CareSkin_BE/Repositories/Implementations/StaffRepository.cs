using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;

namespace SWP391_CareSkin_BE.Repositories
{
    public class StaffRepository : IStaffRepository
    {
        private readonly MyDbContext _context;

        public StaffRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Staff>> GetAllStaffsAsync()
        {
            return await _context.Staffs.ToListAsync();
        }

        public async Task<Staff?> GetStaffByIdAsync(int staffId)
        {
            return await _context.Staffs.FindAsync(staffId);
        }

        public async Task<Staff?> GetStaffByUsernameOrEmailAsync(string username, string email)
        {
            return await _context.Staffs
                .FirstOrDefaultAsync(s => s.UserName == username || s.Email == email);
        }

        public async Task AddStaffAsync(Staff staff)
        {
            await _context.Staffs.AddAsync(staff);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateStaffAsync(Staff staff)
        {
            _context.Staffs.Update(staff);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteStaffAsync(Staff staff)
        {
            _context.Staffs.Remove(staff);
            await _context.SaveChangesAsync();
        }
    }
}
