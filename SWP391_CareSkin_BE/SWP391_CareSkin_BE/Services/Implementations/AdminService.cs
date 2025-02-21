using SWP391_CareSkin_BE.DTOs.Requests.Admin;
using SWP391_CareSkin_BE.DTOs.Responses;
using SWP391_CareSkin_BE.Mappers;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Implementations;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;

        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        } 
        public async Task<List<AdminDTO>> GetAdminAsync()
        {
            var admin = await _adminRepository.GetAdmin();
            return admin.Select(AdminMapper.ToDTO).ToList();
        }

        public async Task<AdminDTO> UpdateAdminAsync(AdminUpdateRequestDTO request, int id)
        {
            var existingAdmin = await _adminRepository.GetAdminByIdAsync(id);
            if (existingAdmin == null) return null;

            AdminMapper.UpdateEnity(request, existingAdmin);
            await _adminRepository.UpdateAdminAsync(existingAdmin);

            var updatedAdmin = await _adminRepository.GetAdminByIdAsync(id);
            return AdminMapper.ToDTO(updatedAdmin);
        }
    }
}
