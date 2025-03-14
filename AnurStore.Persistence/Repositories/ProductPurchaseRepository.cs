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

        public async Task<ProductPurchase> CreateAsync(ProductPurchase productPurchase)
        {
            var result = await _context.AddAsync(productPurchase);
            await _context.SaveChangesAsync();
            return productPurchase; 
        }

        public async Task<IList<ProductPurchase>> GetAllAsync()
        {
            var productPurchase = await _context.ProductPurchases 
           .Where(x => x.IsDeleted == false)
           .ToListAsync();
            return productPurchase;
        }

        public async Task<ProductPurchase> GetByIdAsync(string id)
        {
            return await _context.ProductPurchases.FindAsync(id);
        }

        public async Task<bool> UpdateAsync(ProductPurchase productPurchase)
        {

            var result = _context.ProductPurchases.Update(productPurchase);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
