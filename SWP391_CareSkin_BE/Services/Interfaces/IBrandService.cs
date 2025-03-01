using SWP391_CareSkin_BE.DTOS.Requests;
using SWP391_CareSkin_BE.DTOS.Responses;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface IBrandService
    {
        Task<List<BrandDTO>> GetAllBrandsAsync();
        Task<BrandDTO> GetBrandByIdAsync(int brandId);
        Task<BrandDTO> CreateBrandAsync(BrandCreateRequestDTO request, string pictureUrl);
        Task<BrandDTO> UpdateBrandAsync(int brandId, BrandUpdateRequestDTO request);
        Task<bool> DeleteBrandAsync(int brandId);
    }
}
