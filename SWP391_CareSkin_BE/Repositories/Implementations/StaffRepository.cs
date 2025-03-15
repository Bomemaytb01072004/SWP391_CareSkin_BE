using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.DTOS;
using SWP391_CareSkin_BE.Helpers;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using SWP391_CareSkin_BE.Services;

namespace SWP391_CareSkin_BE.Repositories
{
    public class StaffRepository : IStaffRepository
    {
        private readonly MyDbContext _context;
        private readonly JwtHelper _jwtHelper;

        public StaffRepository(MyDbContext context, JwtHelper jwtHelper)
        {
            _context = context;
            _jwtHelper = jwtHelper;
        }

        public async Task<List<Staff>> GetAllStaffsAsync()
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

        public async Task<Staff?> LoginStaff(LoginDTO request)
        {
            var staff = await _context.Staffs.FirstOrDefaultAsync(a => a.UserName == request.UserName);

            if (staff == null)
            {
                return null;
            }

            if (!Validate.VerifyPassword(staff.Password, request.Password))
            {
                return null;
            }

            string role = "Staff";
            var token = _jwtHelper.GenerateToken(request.UserName, role);
            staff.Token = token;
            staff.Role = role;
            return staff;
        }
    }
}
