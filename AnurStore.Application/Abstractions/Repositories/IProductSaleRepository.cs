using AnurStore.Domain.Entities;

namespace AnurStore.Application.Abstractions.Repositories
{
    public interface IProductSaleRepository
    {
        Task<ProductSale> AddAsync(ProductSale productSale); 
        Task<IList<ProductSale>> GetAllAsync(); 
        Task<ProductSale> GetByIdAsync(string id); 
        Task<bool> UpdateAsync(ProductSale productSale); 
        Task<bool> ExistsAsync(string productSaleName); 
        List<ProductSale> GetAll(); 
    }

}
