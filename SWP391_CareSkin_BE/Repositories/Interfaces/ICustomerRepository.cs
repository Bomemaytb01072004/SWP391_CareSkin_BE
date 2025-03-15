using SWP391_CareSkin_BE.DTOS;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Services;

namespace SWP391_CareSkin_BE.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> GetAllCustomersAsync();
        Task<Customer?> GetCustomerByIdAsync(int customerId);
        Task AddCustomerAsync(Customer customer);
        Task UpdateCustomerAsync(Customer customer);
        Task<Customer?> GetCustomerByEmailOrUsernameAsync(string email, string username);
        Task<Customer> LoginCustomer(LoginDTO request);
        Task<Customer?> GetCustomerByEmailAsync(string email);
    }
}
