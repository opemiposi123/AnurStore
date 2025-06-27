using AnurStore.Application.Pagination;
using AnurStore.Domain.Entities;

namespace AnurStore.Application.Abstractions.Repositories;

public interface IProductPurchaseRepository
{
    Task<ProductPurchase> PurchaseProductAsync(ProductPurchase productPurchase); 
    Task<IList<ProductPurchase>> GetAllAsync();
    Task<ProductPurchase?> GetByIdAsync(string id);
    Task<IList<ProductPurchase>> GetBySupplierIdAsync(string supplierId);
    Task<bool> UpdateAsync(ProductPurchase productPurchase);
    Task<IList<ProductPurchase>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IList<ProductPurchase>> GetPurchasesByProductAsync(string productId);
    Task<IList<ProductPurchase>> GetAllWithDetailsAsync();
    Task<(IList<ProductPurchase> Purchases, int TotalCount)> GetPagedPurchasesAsync(PurchaseFilterRequest filter);
}