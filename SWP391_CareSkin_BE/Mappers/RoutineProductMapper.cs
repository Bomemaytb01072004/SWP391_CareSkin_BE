using SWP391_CareSkin_BE.DTOS.Requests.Routine;
using SWP391_CareSkin_BE.DTOS.Responses.Routine;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Mappers
{
    public static class RoutineProductMapper
    {
        // Chuyển từ Entity RoutineProduct thành DTO
        public static RoutineProductDTO ToDTO(RoutineProduct routineProduct)
        {
            if (routineProduct == null) return null;

            return new RoutineProductDTO
            {
                RoutineProductId = routineProduct.RoutineProductId,
                RoutineId = routineProduct.RoutineId,
                ProductId = routineProduct.ProductId,
                Product = routineProduct.Product != null ? ProductMapper.ToDTO(routineProduct.Product) : null // Chuyển thông tin sản phẩm
            };
        }

        // Chuyển danh sách RoutineProduct thành danh sách DTO
        public static List<RoutineProductDTO> ToDTOList(IEnumerable<RoutineProduct> routineProducts)
        {
            return routineProducts?.Select(ToDTO).ToList() ?? new List<RoutineProductDTO>();
        }

        // Chuyển từ DTO tạo RoutineProduct thành Entity
        public static RoutineProduct ToEntity(RoutineProductCreateRequestDTO dto)
        {
            if (dto == null) return null;

            return new RoutineProduct
            {
                RoutineId = dto.RoutineId,
                ProductId = dto.ProductId
            };
        }

        // Cập nhật Entity RoutineProduct từ DTO cập nhật
        public static void UpdateEntity(RoutineProduct routineProduct, RoutineProductUpdateRequestDTO dto)
        {
            if (routineProduct == null || dto == null) return;

            routineProduct.ProductId = dto.ProductId;
        }
    }
}
