using AnurStore.Application.Abstractions.Repositories;
using AnurStore.Domain.Entities;
using AnurStore.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace AnurStore.Persistence.Repositories
{
    public class ProductSaleRepository : IProductSaleRepository
    {
        private readonly ApplicationContext _context;

        public ProductSaleRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<ProductSale> AddProductSaleAsync(ProductSale productSale)
        {
            var result = await _context.ProductSales.AddAsync(productSale);
            await _context.SaveChangesAsync();
            return productSale;
        }

        public async Task<IList<ProductSale>> GetAllProductSalesAsync()
        {
            var productSales = await _context.ProductSales
                .Include(p => p.ProductSaleItems)
                .ThenInclude(i => i.Product)
                .Where(p => p.IsDeleted == false)
                .ToListAsync();
            return productSales;
        }

        public async Task<ProductSale> GetProductSaleByIdAsync(string id)
        {
            var productSale = await _context.ProductSales
                 .Include(p => p.ProductSaleItems)
                 .ThenInclude(i => i.Product)
                 .FirstOrDefaultAsync(p => p.Id == id);
            return productSale;
        }

        public async Task RemoveProductSaleItemsAsync(IEnumerable<ProductSaleItem> items)
        {
            _context.ProductSaleItems.RemoveRange(items);
            await _context.SaveChangesAsync(); 
        }


        public async Task<bool> UpdateAsync(ProductSale productSale)
        {
            _context.ProductSales.Update(productSale);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}
