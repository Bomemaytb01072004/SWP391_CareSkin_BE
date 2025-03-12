using SWP391_CareSkin_BE.DTOS.Requests.Routine;
using SWP391_CareSkin_BE.DTOS.Responses.Routine;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Mappers
{
    public static class RoutineStepMapper
    {
        public static RoutineStep ToEntity(RoutineStepCreateRequest request)
        {
            return new RoutineStep
            {
                StepNumber = request.StepNumber,
                Description = request.Description,
                RoutineId = request.RoutineId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Status = true
            };
        }

        public static void ToEntity(RoutineStepUpdateRequest request, RoutineStep routineStep)
        {
            routineStep.StepNumber = request.StepNumber;
            routineStep.Description = request.Description;
            routineStep.UpdatedAt = DateTime.Now;
        }

        public static RoutineStepResponse ToResponse(RoutineStep routineStep)
        {
            if (routineStep == null) return null;
            return new RoutineStepResponse
            {
                Id = routineStep.Id,
                StepNumber = routineStep.StepNumber,
                Description = routineStep.Description,
                RoutineId = routineStep.RoutineId,
                CreatedAt = routineStep.CreatedAt,
                UpdatedAt = routineStep.UpdatedAt,
                Status = routineStep.Status,
                Products = routineStep.RoutineProducts?.Select(rp => RoutineProductMapper.ToResponse(rp)).ToList()
            };
        }

        public static RoutineStepDTO ToDTO(this RoutineStep routineStep)
        {
            if (routineStep == null) return null;
            return new RoutineStepDTO
            {
                Id = routineStep.Id,
                StepNumber = routineStep.StepNumber,
                Description = routineStep.Description,
                Products = routineStep.RoutineProducts?.Select(rp => rp.ToDTO()).ToList()
            };
        }
    }
}
