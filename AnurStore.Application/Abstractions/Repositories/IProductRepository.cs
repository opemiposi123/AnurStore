using AnurStore.Domain.Entities;

namespace AnurStore.Application.Abstractions.Repositories
{
    public interface  IProductRepository
    {
        Task<Product> CreateProduct(Product product);
        Task<IList<Product>> GetAllProduct();
        Task<Product> GetProductById(string id);
        Task<bool> UpdateProduct(Product product);
        Task<bool> Exist(string productName);
        List<Product> SelectProduct();
    } 
}
