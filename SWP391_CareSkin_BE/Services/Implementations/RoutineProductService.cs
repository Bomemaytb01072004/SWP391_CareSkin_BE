using SWP391_CareSkin_BE.DTOS.Requests.Routine;
using SWP391_CareSkin_BE.DTOS.Responses.Routine;
using SWP391_CareSkin_BE.Exceptions;
using SWP391_CareSkin_BE.Mappers;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Services.Implementations
{
    public class RoutineProductService : IRoutineProductService
    {
        private readonly IRoutineProductRepository _routineProductRepository;
        private readonly IRoutineRepository _routineRepository;
        private readonly IProductRepository _productRepository;

        public RoutineProductService(
            IRoutineProductRepository routineProductRepository,
            IRoutineRepository routineRepository,
            IProductRepository productRepository)
        {
            _routineProductRepository = routineProductRepository;
            _routineRepository = routineRepository;
            _productRepository = productRepository;
        }

        public async Task<List<RoutineProductDTO>> GetAllRoutineProductsAsync()
        {
            var routineProducts = await _routineProductRepository.GetAllAsync();
            return RoutineProductMapper.ToDTOList(routineProducts);
        }

        public async Task<RoutineProductDTO> GetRoutineProductByIdAsync(int id)
        {
            var routineProduct = await _routineProductRepository.GetByIdAsync(id);
            if (routineProduct == null)
            {
                throw new NotFoundException($"RoutineProduct with ID {id} not found");
            }

            return RoutineProductMapper.ToDTO(routineProduct);
        }

        public async Task<List<RoutineProductDTO>> GetRoutineProductsByRoutineIdAsync(int routineId)
        {
            // Validate routine exists
            var routine = await _routineRepository.GetByIdAsync(routineId);
            if (routine == null)
            {
                throw new NotFoundException($"Routine with ID {routineId} not found");
            }

            var routineProducts = await _routineProductRepository.GetByRoutineIdAsync(routineId);
            return RoutineProductMapper.ToDTOList(routineProducts);
        }

        public async Task<RoutineProductDTO> CreateRoutineProductAsync(RoutineProductCreateRequestDTO request)
        {
            // Validate routine exists
            var routine = await _routineRepository.GetByIdAsync(request.RoutineId);
            if (routine == null)
            {
                throw new NotFoundException($"Routine with ID {request.RoutineId} not found");
            }

            // Validate product exists
            var product = await _productRepository.GetProductByIdAsync(request.ProductId);
            if (product == null)
            {
                throw new NotFoundException($"Product with ID {request.ProductId} not found");
            }

            // Check if the combination already exists
            var existingRoutineProduct = await _routineProductRepository.GetByRoutineIdAndProductIdAsync(
                request.RoutineId, request.ProductId);
            if (existingRoutineProduct != null)
            {
                throw new Exception($"Product with ID {request.ProductId} is already in routine with ID {request.RoutineId}");
            }

            // Create new routine product
            var routineProduct = RoutineProductMapper.ToEntity(request);
            await _routineProductRepository.CreateAsync(routineProduct);

            // Get the created routine product with all related data
            var createdRoutineProduct = await _routineProductRepository.GetByIdAsync(routineProduct.RoutineProductId);
            return RoutineProductMapper.ToDTO(createdRoutineProduct);
        }

        public async Task<RoutineProductDTO> UpdateRoutineProductAsync(int id, RoutineProductUpdateRequestDTO request)
        {
            // Validate routine product exists
            var routineProduct = await _routineProductRepository.GetByIdAsync(id);
            if (routineProduct == null)
            {
                throw new NotFoundException($"RoutineProduct with ID {id} not found");
            }

            // Validate product exists
            var product = await _productRepository.GetProductByIdAsync(request.ProductId);
            if (product == null)
            {
                throw new NotFoundException($"Product with ID {request.ProductId} not found");
            }

            // Check if the new combination already exists (but not this one)
            var existingRoutineProduct = await _routineProductRepository.GetByRoutineIdAndProductIdAsync(
                routineProduct.RoutineId, request.ProductId);
            if (existingRoutineProduct != null && existingRoutineProduct.RoutineProductId != id)
            {
                throw new Exception($"Product with ID {request.ProductId} is already in routine with ID {routineProduct.RoutineId}");
            }

            // Update routine product
            RoutineProductMapper.UpdateEntity(routineProduct, request);
            await _routineProductRepository.UpdateAsync(routineProduct);

            // Get the updated routine product with all related data
            var updatedRoutineProduct = await _routineProductRepository.GetByIdAsync(id);
            return RoutineProductMapper.ToDTO(updatedRoutineProduct);
        }

        public async Task DeleteRoutineProductAsync(int id)
        {
            // Validate routine product exists
            var routineProduct = await _routineProductRepository.GetByIdAsync(id);
            if (routineProduct == null)
            {
                throw new NotFoundException($"RoutineProduct with ID {id} not found");
            }

            // Delete routine product
            await _routineProductRepository.DeleteAsync(routineProduct);
        }
    }
}
