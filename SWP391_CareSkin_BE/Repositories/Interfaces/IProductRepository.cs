using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<List<Product>> GetActiveProductsAsync();
        Task<List<Product>> GetInactiveProductsAsync();
        Task<Product> GetProductByIdAsync(int productId);
        Task<bool> ExistsByNameAsync(string productName);
        Task<Product> GetProductByNameAsync(string productName);
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        IQueryable<Product> GetQueryable();
    }
}
