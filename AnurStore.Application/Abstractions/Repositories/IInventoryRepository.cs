using AnurStore.Domain.Entities;
using AnurStore.Domain.Enums;

namespace AnurStore.Application.Abstractions.Repositories
{
    public interface IInventoryRepository
    {
        Task<Inventory> CreateInventory(Inventory Inventory);
        Task<IList<Inventory>> GetAllInventories();
        Task<Inventory> GetInventoryById(string id);
        List<Inventory> GetAllInventory();
        Task<Inventory?> GetByProductAndBatchAsync(string productId, string batchNumber);
        Task AddAsync(Inventory inventory);
        Task UpdateAsync(Inventory inventory);
        Task<IList<Inventory>> GetByStockStatusAsync(StockStatus status);


    }
}
