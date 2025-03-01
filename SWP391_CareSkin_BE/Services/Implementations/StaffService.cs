using SWP391_CareSkin_BE.DTOS;
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
        private readonly IFirebaseService _firebaseService;

        public StaffService(IStaffRepository staffRepository, IFirebaseService firebaseService)
        {
            _staffRepository = staffRepository;
            _firebaseService = firebaseService;
        }

        public async Task<StaffDTO> RegisterStaffAsync(RegisterStaffDTO request)
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

        public async Task<StaffDTO?> GetStaffByIdAsync(int staffId)
        {
            var staff = await _staffRepository.GetStaffByIdAsync(staffId);
            return staff != null ? StaffMapper.ToStaffResponseDTO(staff) : null;
        }

        public async Task<StaffDTO> UpdateProfileAsync(int staffId, UpdateProfileStaffDTO request, string pictureUrl)
        {
            var staff = await _staffRepository.GetStaffByIdAsync(staffId);
            if (staff == null)
                throw new ArgumentException("Nhân viên không tồn tại.");

            StaffMapper.UpdateStaff(staff, request, pictureUrl);
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

        public async Task<StaffDTO> Login(LoginDTO loginDto)
        {
            var authResult = await _staffRepository.LoginStaff(loginDto);
            return StaffMapper.ToStaffResponseDTO(authResult);
        }

        public Task<List<StaffDTO>> GetAllStaffAsync()
        {
            throw new NotImplementedException();
        }
    }
}
