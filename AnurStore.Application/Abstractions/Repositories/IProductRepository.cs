using AnurStore.Application.DTOs;
using AnurStore.Domain.Entities;

namespace AnurStore.Application.Abstractions.Repositories
{
    public interface IProductRepository
    {
        Task<Product> CreateProduct(Product product);
        Task<IList<Product>> GetAllProduct();
        Task<Product> GetProductById(string id);
        Task<bool> UpdateProduct(Product product);
        Task<bool> Exist(string productName);
        List<Product> SelectProduct(); 
        Task<string> CreateProductWithSizeAsync(Product product, ProductSize productSize);
        Task<ProductSize?> GetProductSizeByProductIdAsync(string productId, bool noTracking = false);
        Task<IEnumerable<ProductDto>> SearchProductsByNameAsync(string query);
        Task<List<Product>> GetProductsByIdsAsync(List<string> productIds);
        Task UpdateRangeAsync(List<Product> products);

    }
}
