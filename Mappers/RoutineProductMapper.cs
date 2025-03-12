using SWP391_CareSkin_BE.DTOS.Requests.Routine;
using SWP391_CareSkin_BE.DTOS.Responses.Routine;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Mappers
{
    public static class RoutineProductMapper
    {
        public static RoutineProduct ToEntity(RoutineProductCreateRequest request)
        {
            return new RoutineProduct
            {
                ProductId = request.ProductId,
                RoutineStepId = request.RoutineStepId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Status = true
            };
        }

        public static void ToEntity(RoutineProductUpdateRequest request, RoutineProduct routineProduct)
        {
            routineProduct.ProductId = request.ProductId;
            routineProduct.UpdatedAt = DateTime.Now;
        }

        public static RoutineProductResponse ToResponse(RoutineProduct routineProduct)
        {
            if (routineProduct == null || routineProduct.Product == null) return null;
            return new RoutineProductResponse
            {
                Id = routineProduct.Id,
                ProductId = routineProduct.ProductId,
                RoutineStepId = routineProduct.RoutineStepId,
                ProductName = routineProduct.Product.Name,
                ProductDescription = routineProduct.Product.Description,
                ProductPrice = routineProduct.Product.Price,
                ProductImageUrl = routineProduct.Product.ImageUrl,
                CreatedAt = routineProduct.CreatedAt,
                UpdatedAt = routineProduct.UpdatedAt,
                Status = routineProduct.Status
            };
        }

        public static RoutineProductDTO ToDTO(this RoutineProduct routineProduct)
        {
            if (routineProduct == null || routineProduct.Product == null) return null;
            return new RoutineProductDTO
            {
                Id = routineProduct.Id,
                Name = routineProduct.Product.Name,
                Description = routineProduct.Product.Description,
                Price = routineProduct.Product.Price,
                ImageUrl = routineProduct.Product.ImageUrl
            };
        }
    }
}
