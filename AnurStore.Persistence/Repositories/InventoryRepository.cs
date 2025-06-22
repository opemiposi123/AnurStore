using AnurStore.Domain.Entities;
using AnurStore.Application.Abstractions.Repositories;
using AnurStore.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using AnurStore.Domain.Enums;

namespace AnurStore.Persistence.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly ApplicationContext _context;

        public InventoryRepository(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<IList<Inventory>> GetByStockStatusAsync(StockStatus status)
        {
            return await _context.Inventories
                .Include(i => i.Product)
                .Where(i => i.StockStatus == status)
                .ToListAsync();
        }

        public async Task<Inventory?> GetByProductAndBatchAsync(string productId, string batchNumber)
        {
            return await _context.Inventories
                .FirstOrDefaultAsync(i => i.ProductId == productId && i.BatchNumber == batchNumber);
        }

        public async Task AddAsync(Inventory inventory)
        {
            await _context.Inventories.AddAsync(inventory);
        }

        public async Task UpdateAsync(Inventory inventory)
        {
            _context.Inventories.Update(inventory);
        }

        public Task<Inventory> CreateInventory(Inventory Inventory)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Inventory>> GetAllInventories()
        {
            throw new NotImplementedException();
        }

        public Task<Inventory> GetInventoryById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateInventory(Inventory inventory)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Exist(string inventoryName)
        {
            throw new NotImplementedException();
        }

        public List<Inventory> GetAllInventory()
        {
            throw new NotImplementedException();
        }
    }

}
