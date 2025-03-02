using SWP391_CareSkin_BE.DTOS.Responses;
using SWP391_CareSkin_BE.DTOs.Requests;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.DTOS.Requests;

namespace SWP391_CareSkin_BE.Mappers
{
    public class StaffMapper
    {
        //chuyển đổi RegisterStaffDTO sang Staff Model
        public static Staff ToStaff (RegisterStaffDTO dto, string hashedPassword)
        {
            return new Staff
            {
                UserName = dto.UserName,
                Password = hashedPassword,
                Email = dto.Email,
                DoB = dto.Dob,
                PictureUrl = dto.ProfilePicture ?? "",
                FullName = dto.FullName ?? "No name",
                Phone = dto.Phone ?? "",
                Token = dto.Token
            };
        }

        //chuyển đổi từ staff model sang staffresponseDTO
        public static StaffDTO ToStaffResponseDTO(Staff staff)
        {
            return new StaffDTO
            {
                StaffId = staff.StaffId,
                UserName = staff.UserName,
                FullName = staff.FullName,
                Email = staff.Email,  
                Phone = staff.Phone,
                DoB = staff.DoB,
                PictureUrl = staff.PictureUrl
            };

        }

        //cập nhật dữ liệu từ DTO vào model
        public static void UpdateStaff(Staff staff, UpdateProfileStaffDTO dto, string pictureUrl = null)
        {
            if (!string.IsNullOrEmpty(staff.FullName))
            {
                staff.FullName = dto.FullName;
            }
            if (!string.IsNullOrEmpty(staff.Email))
            {
                staff.Email = dto.Email;
            }
            if (!string.IsNullOrEmpty(staff.Phone))
            {
                staff.Phone = dto.Phone;
            }
            if (dto.Dob.HasValue)
            {
                staff.DoB = dto.Dob;
            }
            if (!string.IsNullOrEmpty(pictureUrl))
            {
                staff.PictureUrl = pictureUrl;
            }
        }
        

    }
}
