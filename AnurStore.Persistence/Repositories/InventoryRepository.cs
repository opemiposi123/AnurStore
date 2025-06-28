using AnurStore.Domain.Entities;
using AnurStore.Application.Abstractions.Repositories;
using AnurStore.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using AnurStore.Domain.Enums;
using System.Linq;

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
       
        public async Task UpdateAsync(Inventory inventory)
        {
            var tracked = _context.Inventories.Local.FirstOrDefault(e => e.Id == inventory.Id);
            if (tracked != null)
            {
                _context.Entry(tracked).CurrentValues.SetValues(inventory);
            }
            else
            {
                _context.Inventories.Update(inventory);
            }
        }

        public async Task AddAsync(Inventory inventory)
        {
            await _context.Inventories.AddAsync(inventory);
        }

        public async Task<IList<Inventory>> GetAllInventories()
        {
            var inventory = await _context.Inventories
                  .Include(r => r.Product)
                     .ThenInclude(r => r.ProductSize)
                       .ThenInclude(r => r.ProductUnit)
                   .Include(r => r.Product)
                     .ThenInclude(r => r.Category)
                   .Include(r => r.Product)
                     .ThenInclude(r => r.Brand)
                  .ToListAsync();
            return inventory;
        }

        public async Task<Inventory?> GetByProductAndBatchAsync(string productId, string batchNumber)
        {
            return await _context.Inventories
                .FirstOrDefaultAsync(i => i.ProductId == productId && i.BatchNumber == batchNumber);
        }

        public async Task<List<Inventory>> GetByProductsAndBatchAsync(List<string> productIds, string batch)
        {
            return await _context.Inventories
                .Where(i => productIds.Contains(i.ProductId) && i.BatchNumber == batch)
                .ToListAsync();
        }

        public async Task AddRangeAsync(List<Inventory> inventories)
        {
            await _context.Inventories.AddRangeAsync(inventories);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRangeAsync(List<Inventory> inventories)
        {
            _context.Inventories.UpdateRange(inventories);
            await _context.SaveChangesAsync();
        }
    }

}
