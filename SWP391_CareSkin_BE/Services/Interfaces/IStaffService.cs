using SWP391_CareSkin_BE.DTOs.Requests;
using SWP391_CareSkin_BE.DTOS.Requests;
using SWP391_CareSkin_BE.DTOS.Responses;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface IStaffService
    {
        Task<List<StaffResponseDTO>> GetAllStaffAsync();
        Task<StaffResponseDTO> RegisterStaffAsync(RegisterStaffDTO request);
        Task<StaffResponseDTO?> GetStaffByIdAsync(int staffId);
        Task<StaffResponseDTO> UpdateProfileAsync(int staffId, UpdateProfileStaffDTO request);
        Task DeleteStaffAsync(int staffId, string password);
    }
}
