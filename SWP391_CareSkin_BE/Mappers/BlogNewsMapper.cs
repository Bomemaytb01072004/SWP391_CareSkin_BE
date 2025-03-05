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

            return new BlogNewsDTO
            {
                BlogId = blog.BlogId,
                Title = blog.Title,
                Content = blog.Content,
                PictureUrl = blog.PictureUrl,
            };
        }

        // Từ BlogNewsCreateRequestDTO -> Entity
        public static BlogNew ToEntity(BlogNewsCreateRequest request, string pictureUrl = null)
        {
            if (request == null) return null;

            return new BlogNew
            {
                Title = request.Title,
                Content = request.Content,
                PictureUrl = pictureUrl
            };
        }

        public static void UpdateEntity(BlogNew blog, BlogNewsUpdateRequest request)
        {
            if (blog == null || request == null) return;

            blog.Title = request.Title;
            blog.Content = request.Content;
            blog.PictureUrl = request.PictureUrl;
        }


    }
}
