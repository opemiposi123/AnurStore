using AnurStore.Domain.Entities;

namespace AnurStore.Application.Abstractions.Repositories
{
    public interface  IProductRepository
    {
        Task<Product> CreateProduct(Product Product);
        Task<IList<Product>> GetAllProduct();
        Task<Product> GetProductById(string id);
        Task<bool> UpdateProduct(Product Product);
        Task<bool> Exist(string ProductName);
        List<Product> GetAllCategories();
    }
}
