using AnurStore.Domain.Entities;

namespace AnurStore.Application.Abstractions.Repositories
{
    internal interface IInventoryRepository
    {
        Task<Inventory> CreateInventory(Inventory Inventory);
        Task<IList<Inventory>> GetAllInventories();
        Task<Inventory> GetInventoryById(string id);
        Task<bool> UpdateInventory(Inventory inventory);
        Task<bool> Exist(string inventoryName);
        List<Inventory> GetAllInventory();
    }
}
