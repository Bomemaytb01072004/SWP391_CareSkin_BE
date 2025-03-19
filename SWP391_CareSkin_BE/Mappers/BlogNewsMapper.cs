using SWP391_CareSkin_BE.DTOs.Requests.BlogNews;
using SWP391_CareSkin_BE.DTOs.Responses.BlogNews;
using SWP391_CareSkin_BE.DTOS.Requests;
using SWP391_CareSkin_BE.DTOS.Responses;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Mappers
{
    public class BlogNewsMapper
    {
        // Từ Entity -> DTO
        public static BlogNewsDTO ToDTO(BlogNew blog)
        {
            if (blog == null) return null;

            var dto = new BlogNewsDTO
            {
                BlogId = blog.BlogId,
                Title = blog.Title,
                Content = blog.Content,
                PictureUrl = blog.PictureUrl,
                UploadDate = blog.UploadDate,
                IsActive = blog.IsActive
            };

            if (blog.AdminId.HasValue)
            {
                dto.AdminId = blog.AdminId.Value;
            }

            if (blog.StaffId.HasValue)
            {
                dto.StaffId = blog.StaffId.Value;
            }

            return dto;
        }

        // Từ BlogNewsCreateRequestDTO -> Entity
        public static BlogNew ToEntity(BlogNewsCreateRequest request, DateTime date, string pictureUrl = null, int? adminId = null, int? staffId = null)
        {
            if (request == null) return null;

            return new BlogNew
            {
                Title = request.Title,
                Content = request.Content,
                PictureUrl = pictureUrl,
                UploadDate = date,
                AdminId = adminId,     
                StaffId = staffId,
                IsActive = true
            };
        }

        public static void UpdateEntity(BlogNew blog, BlogNewsUpdateRequest request, string pictureUrl = null)
        {
            if (blog == null || request == null) return;

            blog.Title = request.Title;

            blog.Content = request.Content;

            if (pictureUrl != null)
            {
                blog.PictureUrl = pictureUrl;
            }
        }


    }
}