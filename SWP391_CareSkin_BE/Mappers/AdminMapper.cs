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
                ProfilePicture = admin.ProfilePicture,
            };
        }

        // Cập nhật một Admin Entity dựa trên AdminUpdateRequestDTO
        public static void UpdateEntity(AdminUpdateRequestDTO request, Admin admin)
        {
            if (request == null || admin == null) return;

            foreach (var prop in typeof(AdminUpdateRequestDTO).GetProperties())
            {
                if (prop.Name == "ProfilePicture") continue;

                var requestValue = prop.GetValue(request);
                if (requestValue != null)
                {
                    var adminProp = typeof(Admin).GetProperty(prop.Name);

                    if (adminProp != null && adminProp.CanWrite)
                    {
                        adminProp.SetValue(admin, requestValue);
                    }
                }
            }
        } 
    }
}
