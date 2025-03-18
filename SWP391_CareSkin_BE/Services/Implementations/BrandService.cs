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
        private readonly IFirebaseService _firebaseService;

        public BrandService(IBrandRepository brandRepository, IFirebaseService firebaseService)
        {
            _brandRepository = brandRepository;
            _firebaseService = firebaseService;
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
            // Kiểm tra xem Brand với tên này đã tồn tại và đang active chưa
            var existingBrand = await _brandRepository.GetBrandByNameAsync(request.Name);
            
            if (existingBrand != null && existingBrand.IsActive)
            {
                throw new Exception($"Thương hiệu với tên '{request.Name}' đã tồn tại.");
            }

            var brandEntity = BrandMapper.ToEntity(request, pictureUrl);
            await _brandRepository.AddBrandAsync(brandEntity);

            // Lấy lại brand vừa thêm (nếu cần hiển thị ID ...)
            var createdBrand = await _brandRepository.GetBrandByIdAsync(brandEntity.BrandId);
            return BrandMapper.ToDTO(createdBrand);
        }

        public async Task<BrandDTO> UpdateBrandAsync(int brandId, BrandUpdateRequestDTO request, string pictureUrl = null)
        {
            var existingBrand = await _brandRepository.GetBrandByIdAsync(brandId);
            if (existingBrand == null) return null;

            // Update the brand properties
            BrandMapper.UpdateEntity(existingBrand, request);
            
            // Update the picture URL if a new image was uploaded
            if (pictureUrl != null)
            {
                existingBrand.PictureUrl = pictureUrl;
            }

            await _brandRepository.UpdateBrandAsync(existingBrand);

            // Lấy lại brand sau khi update
            var updatedBrand = await _brandRepository.GetBrandByIdAsync(brandId);
            return BrandMapper.ToDTO(updatedBrand);
        }

        public async Task<bool> DeleteBrandAsync(int brandId)
        {
            // Get the brand entity
            var brand = await _brandRepository.GetBrandByIdAsync(brandId);
            if (brand == null) return false;

            // Implement soft delete by setting IsActive to false
            brand.IsActive = false;
            await _brandRepository.UpdateBrandAsync(brand);
            return true;
        }

        public async Task<List<BrandDTO>> GetActiveBrandsAsync()
        {
            var brands = await _brandRepository.GetActiveBrandsAsync();
            return brands.Select(BrandMapper.ToDTO).ToList();
        }

        public async Task<List<BrandDTO>> GetInactiveBrandsAsync()
        {
            var brands = await _brandRepository.GetInactiveBrandsAsync();
            return brands.Select(BrandMapper.ToDTO).ToList();
        }
    }
}
