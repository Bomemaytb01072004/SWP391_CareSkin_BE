﻿using SWP391_CareSkin_BE.DTOs.Requests;
using SWP391_CareSkin_BE.DTOS.Responses;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<List<CustomerDTO>> GetAllCustomersAsync();
        Task<CustomerDTO?> GetCustomerByIdAsync(int customerId);
        Task<CustomerDTO> RegisterCustomerAsync(RegisterCustomerDTO request, string pictureUrl);
        Task<CustomerDTO> UpdateProfileAsync(int customerId, UpdateProfileCustomerDTO request, string pictureUrl);
        Task<bool> DeleteCustomerAsync(int customerId, string password);
    }
}
