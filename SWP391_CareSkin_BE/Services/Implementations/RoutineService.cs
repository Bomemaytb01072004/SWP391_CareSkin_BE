using SWP391_CareSkin_BE.DTOS.Requests.Routine;
using SWP391_CareSkin_BE.DTOS.Responses.Routine;
using SWP391_CareSkin_BE.Exceptions;
using SWP391_CareSkin_BE.Mappers;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Services.Implementations
{
    public class RoutineService : IRoutineService
    {
        private readonly IRoutineRepository _routineRepository;
        private readonly ISkinTypeRepository _skinTypeRepository;

        public RoutineService(IRoutineRepository routineRepository, ISkinTypeRepository skinTypeRepository)
        {
            _routineRepository = routineRepository;
            _skinTypeRepository = skinTypeRepository;
        }

        public async Task<List<RoutineDTO>> GetAllRoutinesAsync()
        {
            var routines = await _routineRepository.GetAllAsync();
            return RoutineMapper.ToDTOList(routines);
        }

        public async Task<RoutineDTO> GetRoutineByIdAsync(int id)
        {
            var routine = await _routineRepository.GetByIdAsync(id);
            if (routine == null)
            {
                throw new NotFoundException($"Routine with ID {id} not found");
            }

            return RoutineMapper.ToDTO(routine);
        }

        public async Task<List<RoutineDTO>> GetRoutineRecommendationsAsync(RoutineRecommendationRequestDTO request)
        {
            // Validate skin type exists
            var skinType = await _skinTypeRepository.GetByIdAsync(request.SkinTypeId);
            if (skinType == null)
            {
                throw new NotFoundException($"SkinType with ID {request.SkinTypeId} not found");
            }

            // Validate period (should be either "morning" or "evening")
            string normalizedPeriod = request.Period.ToLower();
            if (normalizedPeriod != "morning" && normalizedPeriod != "evening")
            {
                throw new Exception("Period must be either 'morning' or 'evening'");
            }

            // Get routines for the specified skin type and period
            var routines = await _routineRepository.GetBySkinTypeAndPeriodAsync(request.SkinTypeId, normalizedPeriod);
            
            // If no routines found, try to find generic routines that work for all skin types
            if (routines == null || !routines.Any())
            {
                // You could implement a fallback strategy here if needed
                // For example, get routines for a "normal" skin type or general routines
            }

            return RoutineMapper.ToDTOList(routines);
        }

        public async Task<RoutineDTO> CreateRoutineAsync(RoutineCreateRequestDTO request)
        {
            // Validate skin type exists
            var skinType = await _skinTypeRepository.GetByIdAsync(request.SkinTypeId);
            if (skinType == null)
            {
                throw new NotFoundException($"SkinType with ID {request.SkinTypeId} not found");
            }

            // Validate period (should be either "morning" or "evening")
            string normalizedPeriod = request.RoutinePeriod.ToLower();
            if (normalizedPeriod != "morning" && normalizedPeriod != "evening")
            {
                throw new Exception("RoutinePeriod must be either 'morning' or 'evening'");
            }

            // Create new routine
            var routine = RoutineMapper.ToEntity(request);
            await _routineRepository.CreateAsync(routine);

            // Get the created routine with all related data
            var createdRoutine = await _routineRepository.GetByIdAsync(routine.RoutineId);
            return RoutineMapper.ToDTO(createdRoutine);
        }

        public async Task<RoutineDTO> UpdateRoutineAsync(int id, RoutineUpdateRequestDTO request)
        {
            // Validate routine exists
            var routine = await _routineRepository.GetByIdAsync(id);
            if (routine == null)
            {
                throw new NotFoundException($"Routine with ID {id} not found");
            }

            // Validate skin type exists
            var skinType = await _skinTypeRepository.GetByIdAsync(request.SkinTypeId);
            if (skinType == null)
            {
                throw new NotFoundException($"SkinType with ID {request.SkinTypeId} not found");
            }

            // Validate period (should be either "morning" or "evening")
            string normalizedPeriod = request.RoutinePeriod.ToLower();
            if (normalizedPeriod != "morning" && normalizedPeriod != "evening")
            {
                throw new Exception("RoutinePeriod must be either 'morning' or 'evening'");
            }

            // Update routine
            RoutineMapper.UpdateEntity(routine, request);
            await _routineRepository.UpdateAsync(routine);

            // Get the updated routine with all related data
            var updatedRoutine = await _routineRepository.GetByIdAsync(id);
            return RoutineMapper.ToDTO(updatedRoutine);
        }

        public async Task DeleteRoutineAsync(int id)
        {
            // Validate routine exists
            var routine = await _routineRepository.GetByIdAsync(id);
            if (routine == null)
            {
                throw new NotFoundException($"Routine with ID {id} not found");
            }

            // Delete routine
            await _routineRepository.DeleteAsync(routine);
        }
    }
}
