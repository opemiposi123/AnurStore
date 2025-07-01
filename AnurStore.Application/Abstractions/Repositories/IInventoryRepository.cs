using AnurStore.Domain.Entities;
using AnurStore.Domain.Enums;

namespace AnurStore.Application.Abstractions.Repositories
{
    public interface IInventoryRepository
    {
        Task<IList<Inventory>> GetAllInventories();
        Task AddAsync(Inventory inventory);
        Task UpdateAsync(Inventory inventory);
        Task<IList<Inventory>> GetByStockStatusAsync(StockStatus status);
        Task<List<Inventory>> GetByProductsAndBatchAsync(List<string> productIds, string batch);
        Task AddRangeAsync(List<Inventory> inventories);
        Task UpdateRangeAsync(List<Inventory> inventories);
        Task<Inventory?> GetByProductAsync(string productId);
    }
}
