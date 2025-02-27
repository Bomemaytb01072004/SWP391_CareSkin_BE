using SWP391_CareSkin_BE.DTOS.Responses;
using SWP391_CareSkin_BE.DTOs.Requests;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.DTOS.Requests;

namespace SWP391_CareSkin_BE.Mappers
{
    public class StaffMapper
    {
        // Chuyển đổi RegisterStaffDTO sang Staff Model
        public static Staff ToStaff(RegisterStaffDTO dto, string hashedPassword)
        {
            return new Staff
            {
                UserName = dto.UserName,
                Password = hashedPassword,
                Email = dto.Email,
                DoB = dto.Dob,
                PictureUrl = dto.ProfilePicture ?? "",
                FullName = dto.FullName ?? "No name",
                Phone = dto.Phone ?? ""
            };
        }

        // Chuyển đổi từ Staff model sang StaffDTO
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

        // Chuyển đổi danh sách Staff sang danh sách StaffDTO
        public static List<StaffDTO> ToStaffResponseDTOList(List<Staff> staffList)
        {
            return staffList.Select(staff => ToStaffResponseDTO(staff)).ToList();
        }

        // Cập nhật dữ liệu từ DTO vào model
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
