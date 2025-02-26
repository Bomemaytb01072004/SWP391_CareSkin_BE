using SWP391_CareSkin_BE.DTOs.Requests;
using SWP391_CareSkin_BE.DTOS.Requests;
using SWP391_CareSkin_BE.DTOS.Responses;
using SWP391_CareSkin_BE.Mappers;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Implementations;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Services
{
    public class StaffService : IStaffService
    {
        private readonly IStaffRepository _staffRepository;

        public StaffService(IStaffRepository staffRepository)
        {
            _staffRepository = staffRepository;
        }
        public async Task<StaffResponseDTO> RegisterStaffAsync(RegisterStaffDTO request)
        {
            if (await _staffRepository.GetStaffByUsernameOrEmailAsync(request.UserName, request.Email) != null)
            {
                throw new ArgumentException("UserName hoặc Email đã tồn tại!");
            }

            if (request.Password != request.ConfirmPassword)
            {
                throw new ArgumentException("Mật khẩu xác nhận không khớp!");
            }

            string hashedPassword = Validate.HashPassword(request.Password);
            var newStaff = StaffMapper.ToStaff(request, hashedPassword);
            await _staffRepository.AddStaffAsync(newStaff);

            return StaffMapper.ToStaffResponseDTO(newStaff);
        }

        public async Task<StaffResponseDTO?> GetStaffByIdAsync(int staffId)
        {
            var staff = await _staffRepository.GetStaffByIdAsync(staffId);
            return staff != null ? StaffMapper.ToStaffResponseDTO(staff) : null;
        }

        public async Task<StaffResponseDTO> UpdateProfileAsync(int staffId, UpdateProfileStaffDTO request)
        {
            var staff = await _staffRepository.GetStaffByIdAsync(staffId);
            if (staff == null)
                throw new ArgumentException("Nhân viên không tồn tại.");

            StaffMapper.UpdateStaff(staff, request);
            await _staffRepository.UpdateStaffAsync(staff);

            return StaffMapper.ToStaffResponseDTO(staff);
        }

        public async Task DeleteStaffAsync(int staffId, string password)
        {
            var staff = await _staffRepository.GetStaffByIdAsync(staffId);
            if (staff == null)
                throw new ArgumentException("Nhân viên không tồn tại.");

            if (!Validate.VerifyPassword(staff.Password, password))
                throw new ArgumentException("Mật khẩu không đúng.");

            await _staffRepository.DeleteStaffAsync(staff);
        }

        public async Task<List<StaffResponseDTO>> GetAllStaffAsync()
        {
            var customers = await _staffRepository.GetAllStaffsAsync();
            return customers.Select(StaffMapper.ToStaffResponseDTO).ToList();
        }
    }
}
