using SWP391_CareSkin_BE.DTOs.Requests;
using SWP391_CareSkin_BE.DTOS.Responses;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Mappers
{
    public class CustomerMapper
    {
        // chuyển đổi từ RegisterDTO sang Customer Model
        public static Customer ToCustomer( RegisterCustomerDTO dto, string hashedPassword)
        {
            return new Customer
            {
                UserName = dto.UserName,
                Password = hashedPassword,
                Email = dto.Email,
                Dob = dto.Dob,
                Phone = dto.Phone ?? "",
                PictureUrl = dto.PictureUrl ?? "",
                Gender = dto.Gender ?? "Unknown",
                Address = dto.Address ?? "Not provided",
                FullName = dto.FullName ?? "No name",
            };
        }


        // chuyển đổi từ Customer Model sang CustomerResponseDTO
        public static CustomerDTO ToCustomerResponseDTO( Customer customer)
        {
            if (customer == null) 
            {
                return null;
            }

            return new CustomerDTO
            {
                CustomerId = customer.CustomerId,
                UserName = customer.UserName,
                Email = customer.Email,
                FullName = customer.FullName,
                Phone = customer.Phone,
                Dob = customer.Dob,
                Gender = customer.Gender,
                PictureUrl = customer.PictureUrl,
                Address = customer.Address,
                token = customer.Token,
                Role = customer.Role,
            };
        }

        // cập nhật dữ liệu từ DTO vào Model
        public static void UpdateCustomer( Customer customer, UpdateProfileCustomerDTO dto, string pictureUrl = null)
        {
            if (!string.IsNullOrEmpty(dto.FullName))
                customer.FullName = dto.FullName;

            if (!string.IsNullOrEmpty(dto.Email))
                customer.Email = dto.Email;

            if (!string.IsNullOrEmpty(dto.Phone))
                customer.Phone = dto.Phone;

            if (dto.Dob.HasValue)
                customer.Dob = dto.Dob.Value;

            if (!string.IsNullOrEmpty(dto.Gender))
                customer.Gender = dto.Gender;

            if (!string.IsNullOrEmpty(pictureUrl))
                customer.PictureUrl = pictureUrl;

            if (!string.IsNullOrEmpty(dto.Address))
                customer.Address = dto.Address;
        }
    }
}
