using AnurStore.Application.Abstractions.Repositories;
using AnurStore.Domain.Entities;

namespace AnurStore.Persistence.Repositories
{
    public class ProductSaleRepository : IProductSaleRepository
    {
        public Task<ProductSale> AddAsync(ProductSale productSale)
        {
            throw new NotImplementedException();
        } 

        public Task<bool> ExistsAsync(string productSaleName)
        {
            throw new NotImplementedException();
        }

        public List<ProductSale> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IList<ProductSale>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ProductSale> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(ProductSale productSale)
        {
            throw new NotImplementedException();
        }
    }
}
