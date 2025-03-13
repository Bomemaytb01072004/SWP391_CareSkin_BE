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
        private readonly IRoutineStepRepository _routineStepRepository;

        public RoutineProductService(
            IRoutineProductRepository routineProductRepository,
            IRoutineRepository routineRepository,
            IProductRepository productRepository,
            IRoutineStepRepository routineStepRepository)
        {
            _routineProductRepository = routineProductRepository;
            _routineRepository = routineRepository;
            _productRepository = productRepository;
            _routineStepRepository = routineStepRepository;
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

            // Get all steps for this routine
            var routineSteps = await _routineStepRepository.GetByRoutineIdAsync(routineId);
            
            // Get all products for each step
            var allProducts = new List<RoutineProduct>();
            foreach (var step in routineSteps)
            {
                var products = await _routineStepRepository.GetProductsByStepIdAsync(step.RoutineStepId);
                allProducts.AddRange(products);
            }

            return RoutineProductMapper.ToDTOList(allProducts);
        }

        public async Task<List<RoutineProductDTO>> GetRoutineProductsByRoutineStepIdAsync(int routineStepId)
        {
            // Check if the routine step exists 
            var routineStep = await _routineStepRepository.GetByIdAsync(routineStepId);
            if (routineStep == null)
            {
                throw new NotFoundException($"RoutineStep with ID {routineStepId} not found");
            }

            var routineProducts = await _routineProductRepository.GetByRoutineStepIdAsync(routineStepId);
            return RoutineProductMapper.ToDTOList(routineProducts);
        }

        public async Task<RoutineProductDTO> CreateRoutineProductAsync(RoutineProductCreateRequestDTO request)
        {
            // Validate routine step exists
            var routineStep = await _routineStepRepository.GetByIdAsync(request.RoutineStepId);
            if (routineStep == null)
            {
                throw new NotFoundException($"RoutineStep with ID {request.RoutineStepId} not found");
            }

            // Validate product exists
            var product = await _productRepository.GetProductByIdAsync(request.ProductId);
            if (product == null)
            {
                throw new NotFoundException($"Product with ID {request.ProductId} not found");
            }

            // Check if the product is already in this step
            var existingRoutineProduct = await _routineProductRepository.GetByStepIdAndProductIdAsync(
                request.RoutineStepId, request.ProductId);
            if (existingRoutineProduct != null)
            {
                throw new BadRequestException($"Product with ID {request.ProductId} is already in step with ID {request.RoutineStepId}");
            }

            // Create new routine product
            var routineProduct = new RoutineProduct
            {
                RoutineStepId = request.RoutineStepId,
                ProductId = request.ProductId
            };
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

            // Validate routine step exists
            var routineStep = await _routineStepRepository.GetByIdAsync(request.RoutineStepId);
            if (routineStep == null)
            {
                throw new NotFoundException($"RoutineStep with ID {request.RoutineStepId} not found");
            }

            // Validate product exists
            var product = await _productRepository.GetProductByIdAsync(request.ProductId);
            if (product == null)
            {
                throw new NotFoundException($"Product with ID {request.ProductId} not found");
            }

            // Check if the product is already in this step (but not this one)
            var existingRoutineProduct = await _routineProductRepository.GetByStepIdAndProductIdAsync(
                request.RoutineStepId, request.ProductId);
            if (existingRoutineProduct != null && existingRoutineProduct.RoutineProductId != id)
            {
                throw new BadRequestException($"Product with ID {request.ProductId} is already in step with ID {request.RoutineStepId}");
            }

            // Update routine product
            routineProduct.RoutineStepId = request.RoutineStepId;
            routineProduct.ProductId = request.ProductId;
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
