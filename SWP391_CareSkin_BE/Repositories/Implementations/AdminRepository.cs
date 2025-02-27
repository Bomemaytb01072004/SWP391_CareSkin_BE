using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;

namespace SWP391_CareSkin_BE.Repositories.Implementations
{
    public class AdminRepository : IAdminRepository
    {
        private readonly MyDbContext _context;

        public AdminRepository(MyDbContext context)
        {
            _context = context;
        } 
        public async Task<List<Admin>> GetAdmin()
        {
            return await _context.Admins.ToListAsync();
        }

        public Task<Admin> GetAdminByIdAsync(int adminId)
        {
            return _context.Admins.FirstOrDefaultAsync(aId => aId.AdminId == adminId);
        }

        public async Task UpdateAdminAsync(Admin admin)
        {
            _context.Admins.Update(admin);
            await _context.SaveChangesAsync();
        }
    }
}
