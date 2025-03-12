using SWP391_CareSkin_BE.DTOS.Requests.Routine;
using SWP391_CareSkin_BE.DTOS.Responses.Routine;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Mappers
{
    public static class RoutineMapper
    {
        public static RoutineDTO ToDTO(Routine routine)
        {
            if (routine == null) return null;

            return new RoutineDTO
            {
                RoutineId = routine.RoutineId,
                RoutineName = routine.RoutineName,
                RoutinePeriod = routine.RoutinePeriod,
                Description = routine.Description,
                SkinTypeId = routine.SkinTypeId,
                SkinTypeName = routine.SkinType?.TypeName,
                RoutineProducts = routine.RoutineProducts?
                    .Select(rp => RoutineProductMapper.ToDTO(rp))
                    .ToList() ?? new List<RoutineProductDTO>()
            };
        }

        public static List<RoutineDTO> ToDTOList(IEnumerable<Routine> routines)
        {
            return routines?.Select(ToDTO).ToList() ?? new List<RoutineDTO>();
        }

        public static Routine ToEntity(RoutineCreateRequestDTO dto)
        {
            if (dto == null) return null;

            return new Routine
            {
                RoutineName = dto.RoutineName,
                RoutinePeriod = dto.RoutinePeriod,
                Description = dto.Description,
                SkinTypeId = dto.SkinTypeId
            };
        }

        public static void UpdateEntity(Routine routine, RoutineUpdateRequestDTO dto)
        {
            if (routine == null || dto == null) return;

            routine.RoutineName = dto.RoutineName;
            routine.RoutinePeriod = dto.RoutinePeriod;
            routine.Description = dto.Description;
            routine.SkinTypeId = dto.SkinTypeId;
        }
    }
}
