using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Repositories.Interfaces
{
    public interface IAdminRepository
    {
        Task<List<Admin>> GetAdmin();
        Task<Admin> GetAdminByIdAsync(int adminId);
        Task UpdateAdminAsync(Admin admin);
    }
}
