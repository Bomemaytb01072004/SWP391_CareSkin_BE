using SWP391_CareSkin_BE.DTOs.Requests;
using SWP391_CareSkin_BE.DTOS;
using SWP391_CareSkin_BE.DTOS.Responses;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<List<CustomerDTO>> GetAllCustomersAsync();
        Task<CustomerDTO?> GetCustomerByIdAsync(int customerId);
        Task<CustomerDTO> RegisterCustomerAsync(RegisterCustomerDTO request);
        Task<CustomerDTO> UpdateProfileAsync(int customerId, UpdateProfileCustomerDTO request, string pictureUrl);
        Task<bool> DeleteCustomerAsync(int customerId, string password);
        Task<CustomerDTO> Login(LoginDTO loginDto);
        Task<Customer?> GetCustomerByEmailAsync(string email);
        Task<CustomerDTO> CreateGoogleUserAsync(Customer customer);
    }
}
