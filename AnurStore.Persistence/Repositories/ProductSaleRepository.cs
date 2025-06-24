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

        public async Task<List<string>> GetTopSoldProductsRawAsync(DateTime startDate, DateTime endDate, int limit = 9)
        {
            var result = await _context.ProductSaleItems
                .Where(x => x.ProductSale.SaleDate >= startDate && x.ProductSale.SaleDate <= endDate)
                .OrderByDescending(x => x.ProductSale.SaleDate) 
                .Select(x => x.ProductId)
                .Distinct()
                .Take(limit)
                .ToListAsync();

            return result;
        }




        public async Task RemoveProductSaleItemsAsync(IEnumerable<ProductSaleItem> items)
        {
            _context.ProductSaleItems.RemoveRange(items);
            await _context.SaveChangesAsync(); 
        }

        public async Task<int> GetTotalProductSalesCountAsync()
        {
            return await _context.ProductSales.CountAsync();
        }


        public async Task<List<ProductSale>> GetProductSalesPagedAsync(int pageNumber, int pageSize)
        {
            return await _context.ProductSales
                .Include(s => s.ProductSaleItems)
                .ThenInclude(item => item.Product)
                .OrderByDescending(s => s.SaleDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }




        public async Task<bool> UpdateAsync(ProductSale productSale)
        {
            _context.ProductSales.Update(productSale);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}
