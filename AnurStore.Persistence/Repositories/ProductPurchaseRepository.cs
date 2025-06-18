using AnurStore.Application.Abstractions.Repositories;
using AnurStore.Domain.Entities;
using AnurStore.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Drawing2D;

namespace AnurStore.Persistence.Repositories
{
    public class ProductPurchaseRepository : IProductPurchaseRepository
    {
        private readonly ApplicationContext _context;

        public ProductPurchaseRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<ProductPurchase> PurchaseProductAsync(ProductPurchase productPurchase)
        {
            await _context.ProductPurchases.AddAsync(productPurchase);
            await _context.SaveChangesAsync();
            return productPurchase;
        }

        public async Task<IList<ProductPurchase>> GetAllAsync()
        {
            return await _context.ProductPurchases
                .Include(p => p.PurchaseItems) 
                  .ThenInclude(p => p.Product)
                .OrderByDescending(p => p.PurchaseDate)
                .ToListAsync();
        }

        public async Task<ProductPurchase> GetByIdAsync(string id)
        {
            return await _context.ProductPurchases
                .Include(p => p.PurchaseItems)
                  .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        //public async Task<IList<ProductPurchase>> GetByProductIdAsync(string productId)
        //{
        //    return await _context.ProductPurchases
        //        .Where(p => p.PurchaseItems.ProductId == productId)
        //        .OrderByDescending(p => p.PurchaseDate)
        //        .ToListAsync();
        //}

        public async Task<bool> UpdateAsync(ProductPurchase productPurchase)
        {
            var existing = await _context.ProductPurchases.FindAsync(productPurchase.Id);
            if (existing == null)
                return false;

            _context.Entry(existing).CurrentValues.SetValues(productPurchase);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
