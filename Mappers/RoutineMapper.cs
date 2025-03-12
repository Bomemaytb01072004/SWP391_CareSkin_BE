using SWP391_CareSkin_BE.DTOS.Requests.Routine;
using SWP391_CareSkin_BE.DTOS.Responses.Routine;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Mappers
{
    public static class RoutineMapper
    {
        public static Routine ToEntity(RoutineCreateRequest request)
        {
            return new Routine
            {
                Name = request.Name,
                Description = request.Description,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Status = true
            };
        }

        public static void ToEntity(RoutineUpdateRequest request, Routine routine)
        {
            routine.Name = request.Name;
            routine.Description = request.Description;
            routine.UpdatedAt = DateTime.Now;
        }

        public static RoutineResponse ToResponse(Routine routine)
        {
            if (routine == null) return null;
            return new RoutineResponse
            {
                Id = routine.Id,
                Name = routine.Name,
                Description = routine.Description,
                CreatedAt = routine.CreatedAt,
                UpdatedAt = routine.UpdatedAt,
                Status = routine.Status,
                Steps = routine.RoutineSteps?.OrderBy(s => s.StepNumber).Select(s => RoutineStepMapper.ToResponse(s)).ToList()
            };
        }

        public static RoutineDTO ToDTO(Routine routine)
        {
            if (routine == null) return null;
            return new RoutineDTO
            {
                Id = routine.Id,
                Name = routine.Name,
                Description = routine.Description,
                Steps = routine.RoutineSteps?.OrderBy(s => s.StepNumber).Select(s => s.ToDTO()).ToList()
            };
        }
    }
}
