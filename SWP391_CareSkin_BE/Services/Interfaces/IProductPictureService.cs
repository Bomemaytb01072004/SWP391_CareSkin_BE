using Microsoft.AspNetCore.Http;
using SWP391_CareSkin_BE.DTOS.ProductPicture;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface IProductPictureService
    {
        Task<IEnumerable<ProductPictureDTO>> GetAllProductPicturesAsync();
        Task<IEnumerable<ProductPictureDTO>> GetProductPicturesByProductIdAsync(int productId);
        Task<ProductPictureDTO> GetProductPictureByIdAsync(int id);
        Task<ProductPictureDTO> CreateProductPictureAsync(CreateProductPictureDTO createDto);
        Task<ProductPictureDTO> UpdateProductPictureAsync(int id, UpdateProductPictureDTO updateDto);
        Task<bool> DeleteProductPictureAsync(int id);
        Task<bool> DeleteProductPicturesByProductIdAsync(int productId);
    }
}
