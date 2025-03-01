using SWP391_CareSkin_BE.DTOs.Requests;
using SWP391_CareSkin_BE.DTOs.Responses;
using SWP391_CareSkin_BE.DTOS;
using SWP391_CareSkin_BE.DTOS.Requests;
using SWP391_CareSkin_BE.DTOS.Responses;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface IStaffService
    {
        Task<StaffDTO> RegisterStaffAsync(RegisterStaffDTO request);
        Task<List<StaffDTO>> GetAllStaffAsync();
        Task<StaffDTO?> GetStaffByIdAsync(int staffId);
        Task<StaffDTO> UpdateProfileAsync(int staffId, UpdateProfileStaffDTO request, string pictureUrl);
        Task DeleteStaffAsync(int staffId, string password);
        Task<StaffDTO> Login(LoginDTO loginDto);        
    }
}
