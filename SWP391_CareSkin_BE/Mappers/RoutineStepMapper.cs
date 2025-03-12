using SWP391_CareSkin_BE.DTOS.Requests.Routine;
using SWP391_CareSkin_BE.DTOS.Responses.Routine;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Mappers
{
    public static class RoutineStepMapper
    {
        public static RoutineStepDTO ToDTO(RoutineStep routineStep)
        {
            if (routineStep == null)
                return null;

            return new RoutineStepDTO
            {
                RoutineStepId = routineStep.RoutineStepId,
                RoutineId = routineStep.RoutineId,
                RoutineProductId = routineStep.RoutineProductId,
                StepOrder = routineStep.StepOrder,
                StepName = routineStep.StepName,
                Description = routineStep.Description,
                RoutineName = routineStep.Routine?.RoutineName,
                Product = routineStep.RoutineProduct?.Product != null ? ProductMapper.ToDTO(routineStep.RoutineProduct.Product) : null
            };
        }

        public static List<RoutineStepDTO> ToDTOList(IEnumerable<RoutineStep> routineSteps)
        {
            return routineSteps?.Select(ToDTO).ToList() ?? new List<RoutineStepDTO>();
        }

        public static RoutineStep ToEntity(RoutineStepCreateRequestDTO requestDTO)
        {
            return new RoutineStep
            {
                RoutineId = requestDTO.RoutineId,
                RoutineProductId = requestDTO.RoutineProductId,
                StepOrder = requestDTO.StepOrder,
                StepName = requestDTO.StepName,
                Description = requestDTO.Description
            };
        }

        public static void UpdateEntity(RoutineStep routineStep, RoutineStepUpdateRequestDTO requestDTO)
        {
            routineStep.RoutineProductId = requestDTO.RoutineProductId;
            routineStep.StepOrder = requestDTO.StepOrder;
            routineStep.StepName = requestDTO.StepName;
            routineStep.Description = requestDTO.Description;
        }
    }
}
