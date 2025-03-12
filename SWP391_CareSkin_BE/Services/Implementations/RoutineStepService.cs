using SWP391_CareSkin_BE.DTOS.Requests.Routine;
using SWP391_CareSkin_BE.DTOS.Responses.Routine;
using SWP391_CareSkin_BE.Exceptions;
using SWP391_CareSkin_BE.Mappers;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Services.Implementations
{
    public class RoutineStepService : IRoutineStepService
    {
        private readonly IRoutineStepRepository _routineStepRepository;
        private readonly IRoutineRepository _routineRepository;
        private readonly IRoutineProductRepository _routineProductRepository;

        public RoutineStepService(
            IRoutineStepRepository routineStepRepository,
            IRoutineRepository routineRepository,
            IRoutineProductRepository routineProductRepository)
        {
            _routineStepRepository = routineStepRepository;
            _routineRepository = routineRepository;
            _routineProductRepository = routineProductRepository;
        }

        public async Task<List<RoutineStepDTO>> GetAllRoutineStepsAsync()
        {
            var routineSteps = await _routineStepRepository.GetAllAsync();
            return RoutineStepMapper.ToDTOList(routineSteps);
        }

        public async Task<RoutineStepDTO> GetRoutineStepByIdAsync(int id)
        {
            var routineStep = await _routineStepRepository.GetByIdAsync(id);
            if (routineStep == null)
            {
                throw new NotFoundException($"RoutineStep with ID {id} not found");
            }

            return RoutineStepMapper.ToDTO(routineStep);
        }

        public async Task<List<RoutineStepDTO>> GetRoutineStepsByRoutineIdAsync(int routineId)
        {
            // Validate routine exists
            var routine = await _routineRepository.GetByIdAsync(routineId);
            if (routine == null)
            {
                throw new NotFoundException($"Routine with ID {routineId} not found");
            }

            var routineSteps = await _routineStepRepository.GetByRoutineIdAsync(routineId);
            return RoutineStepMapper.ToDTOList(routineSteps);
        }

        public async Task<RoutineStepDTO> CreateRoutineStepAsync(RoutineStepCreateRequestDTO request)
        {
            // Validate routine exists
            var routine = await _routineRepository.GetByIdAsync(request.RoutineId);
            if (routine == null)
            {
                throw new NotFoundException($"Routine with ID {request.RoutineId} not found");
            }

            // Validate routineProduct exists and belongs to the routine
            var routineProduct = await _routineProductRepository.GetByIdAsync(request.RoutineProductId);
            if (routineProduct == null)
            {
                throw new NotFoundException($"RoutineProduct with ID {request.RoutineProductId} not found");
            }

            if (routineProduct.RoutineId != request.RoutineId)
            {
                throw new BadRequestException($"RoutineProduct with ID {request.RoutineProductId} does not belong to Routine with ID {request.RoutineId}");
            }

            // Check if step order already exists for this routine
            var existingStep = await _routineStepRepository.GetByRoutineIdAndOrderAsync(
                request.RoutineId, request.StepOrder);
            if (existingStep != null)
            {
                throw new BadRequestException($"Step order {request.StepOrder} already exists for Routine with ID {request.RoutineId}");
            }

            // Create new routine step
            var routineStep = RoutineStepMapper.ToEntity(request);
            await _routineStepRepository.CreateAsync(routineStep);

            // Get the created routine step with all related data
            var createdRoutineStep = await _routineStepRepository.GetByIdAsync(routineStep.RoutineStepId);
            return RoutineStepMapper.ToDTO(createdRoutineStep);
        }

        public async Task<RoutineStepDTO> UpdateRoutineStepAsync(int id, RoutineStepUpdateRequestDTO request)
        {
            // Validate routine step exists
            var routineStep = await _routineStepRepository.GetByIdAsync(id);
            if (routineStep == null)
            {
                throw new NotFoundException($"RoutineStep with ID {id} not found");
            }

            // Validate routineProduct exists and belongs to the routine
            var routineProduct = await _routineProductRepository.GetByIdAsync(request.RoutineProductId);
            if (routineProduct == null)
            {
                throw new NotFoundException($"RoutineProduct with ID {request.RoutineProductId} not found");
            }

            if (routineProduct.RoutineId != routineStep.RoutineId)
            {
                throw new BadRequestException($"RoutineProduct with ID {request.RoutineProductId} does not belong to Routine with ID {routineStep.RoutineId}");
            }

            // Check if step order already exists for this routine (but not this one)
            var existingStep = await _routineStepRepository.GetByRoutineIdAndOrderAsync(
                routineStep.RoutineId, request.StepOrder);
            if (existingStep != null && existingStep.RoutineStepId != id)
            {
                throw new BadRequestException($"Step order {request.StepOrder} already exists for Routine with ID {routineStep.RoutineId}");
            }

            // Update routine step
            RoutineStepMapper.UpdateEntity(routineStep, request);
            await _routineStepRepository.UpdateAsync(routineStep);

            // Get the updated routine step with all related data
            var updatedRoutineStep = await _routineStepRepository.GetByIdAsync(id);
            return RoutineStepMapper.ToDTO(updatedRoutineStep);
        }

        public async Task DeleteRoutineStepAsync(int id)
        {
            // Validate routine step exists
            var routineStep = await _routineStepRepository.GetByIdAsync(id);
            if (routineStep == null)
            {
                throw new NotFoundException($"RoutineStep with ID {id} not found");
            }

            // Delete routine step
            await _routineStepRepository.DeleteAsync(routineStep);
        }
    }
}
