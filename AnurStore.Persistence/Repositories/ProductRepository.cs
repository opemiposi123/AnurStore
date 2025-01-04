using AnurStore.Application.Abstractions.Repositories;
using AnurStore.Domain.Entities;

namespace AnurStore.Persistence.Repositories
{
    public class ProductRepository : IProductRepository
    {
        public Task<Product> CreateProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Exist(string productName)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Product>> GetAllProduct()
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetProductById(string id)
        {
            throw new NotImplementedException();
        }

        public List<Product> SelectProduct()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateProduct(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
