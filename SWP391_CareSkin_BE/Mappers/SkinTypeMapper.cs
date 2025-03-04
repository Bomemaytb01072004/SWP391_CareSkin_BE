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
                Description = skinType.Description
            };
        }

        public static SkinType ToEntity(SkinTypeCreateRequestDTO request)
        {
            if (request == null)
                return null;

            return new SkinType
            {
                TypeName = request.TypeName,
                Description = request.Description
            };
        }

        public static void UpdateEntity(SkinType skinType, SkinTypeUpdateRequestDTO request)
        {
            if (skinType == null || request == null)
                return;

            skinType.TypeName = request.TypeName;
            skinType.Description = request.Description;
        }
    }
}
