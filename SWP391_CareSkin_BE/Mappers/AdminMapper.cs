using SWP391_CareSkin_BE.DTOs.Requests.Admin;
using SWP391_CareSkin_BE.DTOs.Responses;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Mappers
{
    public class AdminMapper
    {
        // Từ Entity -> DTO
        public static AdminDTO ToDTO(Admin admin)
        {
            if (admin == null) return null;

            return new AdminDTO
            {
                AdminId = admin.AdminId,
                UserName = admin.UserName,
                Password = admin.Password,
                FullName = admin.FullName,
                Email = admin.Email,
                Phone = admin.Phone,
                DoB = admin.DoB,
                PictureUrl = admin.PictureUrl,
            };
        }

        // Cập nhật một Admin Entity dựa trên AdminUpdateRequestDTO
        public static void UpdateEntity(AdminUpdateRequestDTO request, Admin admin, string pictureUrl = null)
        {
            if (request == null || admin == null) return;

            admin.Password = request.Password;
            admin.FullName = request.FullName;
            admin.Email = request.Email;
            admin.Phone = request.Phone;
            admin.DoB = request.DoB;
            admin.PictureUrl = pictureUrl;
        }
    }
}
