using SWP391_CareSkin_BE.DTOs.Requests;
using SWP391_CareSkin_BE.DTOS;
using SWP391_CareSkin_BE.DTOS.Responses;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<List<CustomerResponseDTO>> GetAllCustomersAsync();
        Task<CustomerResponseDTO?> GetCustomerByIdAsync(int customerId);
        Task<CustomerResponseDTO> RegisterCustomerAsync(RegisterDTO request);
        Task<CustomerResponseDTO> UpdateProfileAsync(int customerId, UpdateProfileCustomerDTO request);
        Task<bool> DeleteCustomerAsync(int customerId, string password);
        Task<LoginResult> Login(LoginDTO loginDto);

    }
}
