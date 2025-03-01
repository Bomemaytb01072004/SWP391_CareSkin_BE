using SWP391_CareSkin_BE.DTOS;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Services;

namespace SWP391_CareSkin_BE.Repositories.Interfaces
{
    public interface IAdminRepository
    {
        Task<List<Admin>> GetAdmin();
        Task<Admin> GetAdminByIdAsync(int adminId);
        Task UpdateAdminAsync(Admin admin);
        Task<string> UploadImageAsync(IFormFile image);
        void DeleteOldImage(string imageUrl);
        Task<Admin> LoginAdmin(LoginDTO request);
    }
}
