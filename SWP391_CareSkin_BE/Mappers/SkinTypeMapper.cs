using SWP391_CareSkin_BE.DTOS.Requests;
using SWP391_CareSkin_BE.DTOS.Responses;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Mappers
{
    public class SkinTypeMapper
    {
        public static SkinTypeDTO ToDTO(SkinType skinType)
        {
            if (skinType == null)
                return null;

            return new SkinTypeDTO
            {
                SkinTypeId = skinType.SkinTypeId,
                TypeName = skinType.TypeName,
                MinScore = skinType.MinScore,
                MaxScore = skinType.MaxScore,
                Description = skinType.Description,
                IsActive = skinType.IsActive 
            };
        }

        public static SkinType ToEntity(SkinTypeCreateRequestDTO request)
        {
            if (request == null)
                return null;

            return new SkinType
            {
                TypeName = request.TypeName,
                MinScore = request.MinScore,
                MaxScore = request.MaxScore,
                Description = request.Description,
                IsActive = true 
            };
        }

        public static void UpdateEntity(SkinType skinType, SkinTypeUpdateRequestDTO request)
        {
            if (skinType == null || request == null)
                return;

            skinType.TypeName = request.TypeName;
            skinType.MinScore = request.MinScore;
            skinType.MaxScore = request.MaxScore;
            skinType.Description = request.Description;
        }
    }
}
