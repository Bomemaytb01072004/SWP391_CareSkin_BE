using SWP391_CareSkin_BE.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391_CareSkin_BE.Repositories.Interfaces
{
    public interface IProductPictureRepository
    {
        Task<IEnumerable<ProductPicture>> GetAllProductPicturesAsync();
        Task<IEnumerable<ProductPicture>> GetProductPicturesByProductIdAsync(int productId);
        Task<ProductPicture> GetProductPictureByIdAsync(int id);
        Task CreateProductPictureAsync(ProductPicture productPicture);
        Task UpdateProductPictureAsync(ProductPicture productPicture);
        Task<bool> DeleteProductPictureAsync(int id);
        Task<bool> DeleteProductPicturesByProductIdAsync(int productId);
    }
}
