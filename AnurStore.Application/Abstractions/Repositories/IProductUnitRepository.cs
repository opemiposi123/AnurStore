using AnurStore.Domain.Entities;

namespace AnurStore.Application.Abstractions.Repositories
{
    public interface IProductUnitRepository
    {
        Task<ProductUnit> CreateProductUnit(ProductUnit productUnit);
        Task<IList<ProductUnit>> GetAllProductUnit(); 
        Task<ProductUnit> GetProductUnitById(string id);
        Task<bool> UpdateProductUnit(ProductUnit productUnit);
        Task<bool> Exist(string productUnitName);
        List<ProductUnit> SelectProductUnit();
        Task<ProductUnit?> GetProductUnitByNameAsync(string unitName);
    }
}  