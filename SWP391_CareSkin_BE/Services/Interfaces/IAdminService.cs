using SWP391_CareSkin_BE.DTOs.Requests.Admin;
using SWP391_CareSkin_BE.DTOs.Responses;
using SWP391_CareSkin_BE.DTOS;
using SWP391_CareSkin_BE.Mappers;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface IAdminService
    {
        Task<List<AdminDTO>> GetAdminAsync();
        Task<AdminDTO> UpdateAdminAsync(AdminUpdateRequestDTO request, int id);
        Task<LoginResult> Login(LoginDTO loginDto);
    }
}
