using AnurStore.Domain.Entities;

namespace AnurStore.Application.Abstractions.Repositories
{
    public interface IProductSaleRepository
    {
        Task<ProductSale> AddProductSaleAsync(ProductSale productSale);
        Task<IList<ProductSale>> GetAllProductSalesAsync();
        Task<ProductSale> GetProductSaleByIdAsync(string id);
        Task<bool> UpdateAsync(ProductSale productSale);
        Task RemoveProductSaleItemsAsync(IEnumerable<ProductSaleItem> items);
    }

}
