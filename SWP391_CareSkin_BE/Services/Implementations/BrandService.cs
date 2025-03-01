using SWP391_CareSkin_BE.DTOS.Requests;
using SWP391_CareSkin_BE.DTOS.Responses;
using SWP391_CareSkin_BE.Mappers;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using SWP391_CareSkin_BE.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace SWP391_CareSkin_BE.Services.Implementations
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;

        public BrandService(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task<List<BrandDTO>> GetAllBrandsAsync()
        {
            var brands = await _brandRepository.GetAllBrandsAsync();
            return brands.Select(BrandMapper.ToDTO).ToList();
        }

        public async Task<BrandDTO> GetBrandByIdAsync(int brandId)
        {
            var brand = await _brandRepository.GetBrandByIdAsync(brandId);
            return BrandMapper.ToDTO(brand);
        }

        public async Task<BrandDTO> CreateBrandAsync(BrandCreateRequestDTO request, string pictureUrl)
        {
            var brandEntity = BrandMapper.ToEntity(request, pictureUrl);
            await _brandRepository.AddBrandAsync(brandEntity);

            // Lấy lại brand vừa thêm (nếu cần hiển thị ID ...)
            var createdBrand = await _brandRepository.GetBrandByIdAsync(brandEntity.BrandId);
            return BrandMapper.ToDTO(createdBrand);
        }

        public async Task<BrandDTO> UpdateBrandAsync(int brandId, BrandUpdateRequestDTO request)
        {
            var existingBrand = await _brandRepository.GetBrandByIdAsync(brandId);
            if (existingBrand == null) return null;

            BrandMapper.UpdateEntity(existingBrand, request);
            await _brandRepository.UpdateBrandAsync(existingBrand);

            // Lấy lại brand sau khi update
            var updatedBrand = await _brandRepository.GetBrandByIdAsync(brandId);
            return BrandMapper.ToDTO(updatedBrand);
        }

        public async Task<bool> DeleteBrandAsync(int brandId)
        {
            await _brandRepository.DeleteBrandAsync(brandId);
            return true;
        }
    }
}
