using SWP391_CareSkin_BE.DTOS.Requests.Routine;
using SWP391_CareSkin_BE.DTOS.Responses.Routine;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Mappers
{
    public static class RoutineStepMapper
    {
        // Chuyển từ Entity RoutineStep thành DTO
        public static RoutineStepDTO ToDTO(RoutineStep routineStep)
        {
            if (routineStep == null)
                return null;

            return new RoutineStepDTO
            {
                RoutineStepId = routineStep.RoutineStepId,
                RoutineId = routineStep.RoutineId,
                StepOrder = routineStep.StepOrder,
                StepName = routineStep.StepName,
                Description = routineStep.Description,

                // Nếu có nhiều RoutineProduct liên kết với step này, map chúng vào List<RoutineProductDTO>
                RoutineProducts = routineStep.RoutineProducts?
                    .Select(rp => RoutineProductMapper.ToDTO(rp)) // Chuyển mỗi RoutineProduct thành DTO
                    .ToList() ?? new List<RoutineProductDTO>()
            };
        }

        // Chuyển danh sách RoutineStep thành danh sách DTO
        public static List<RoutineStepDTO> ToDTOList(IEnumerable<RoutineStep> routineSteps)
        {
            return routineSteps?.Select(ToDTO).ToList() ?? new List<RoutineStepDTO>();
        }

        // Chuyển từ DTO tạo RoutineStep thành Entity
        public static RoutineStep ToEntity(RoutineStepCreateRequestDTO requestDTO)
        {
            if (requestDTO == null) return null;

            return new RoutineStep
            {
                RoutineId = requestDTO.RoutineId,
                StepOrder = requestDTO.StepOrder,
                StepName = requestDTO.StepName,
                Description = requestDTO.Description
            };
        }

        // Cập nhật Entity RoutineStep từ DTO cập nhật
        public static void UpdateEntity(RoutineStep routineStep, RoutineStepUpdateRequestDTO requestDTO)
        {
            routineStep.StepOrder = requestDTO.StepOrder;
            routineStep.StepName = requestDTO.StepName;
            routineStep.Description = requestDTO.Description;
        }
    }
}
