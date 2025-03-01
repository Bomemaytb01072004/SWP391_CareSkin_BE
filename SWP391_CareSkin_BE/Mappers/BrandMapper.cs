using SWP391_CareSkin_BE.DTOS.Requests;
using SWP391_CareSkin_BE.DTOS.Responses;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Mappers
{
    public class BrandMapper
    {
        // Từ Entity -> DTO
        public static BrandDTO ToDTO(Brand brand)
        {
            if (brand == null) return null;

            return new BrandDTO
            {
                BrandId = brand.BrandId,
                Name = brand.Name,
                PictureUrl = brand.PictureUrl
            };
        }

        // Từ BrandCreateRequestDTO -> Entity
        public static Brand ToEntity(BrandCreateRequestDTO request, string pictureUrl = null)
        {
            if (request == null) return null;

            return new Brand
            {
                Name = request.Name,
                PictureUrl = pictureUrl
            };
        }

        // Cập nhật Entity dựa trên BrandUpdateRequestDTO (nếu có)
        public static void UpdateEntity(Brand brand, BrandUpdateRequestDTO request)
        {
            if (brand == null || request == null) return;
            brand.Name = request.Name;
        }
    }
}
