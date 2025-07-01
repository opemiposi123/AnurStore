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
        Task<int> GetTotalProductSalesCountAsync(string username = null);
        Task<List<ProductSale>> GetProductSalesPagedAsync(int pageNumber, int pageSize, string username = null);
        Task<List<string>> GetTopSoldProductsRawAsync(DateTime startDate, DateTime endDate, int limit = 9);
    }

}
