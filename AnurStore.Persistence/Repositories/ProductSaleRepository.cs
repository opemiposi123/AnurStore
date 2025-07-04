﻿using AnurStore.Application.Abstractions.Repositories;
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
                   .ThenInclude(p => p.Inventory)
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

     
        public async Task<List<ProductSale>> GetProductSalesPagedAsync(int pageNumber, int pageSize, string username = null)
        {
            IQueryable<ProductSale> query = _context.ProductSales
                .Include(s => s.ProductSaleItems)
                .ThenInclude(item => item.Product);

            if (!string.IsNullOrEmpty(username))
            {
                query = query.Where(sale => sale.CreatedBy == username);
            }

            return await query
                .OrderByDescending(sale => sale.SaleDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalProductSalesCountAsync(string username = null)
        {
            IQueryable<ProductSale> query = _context.ProductSales;

            if (!string.IsNullOrEmpty(username))
            {
                query = query.Where(sale => sale.CreatedBy == username);
            }

            return await query.CountAsync();
        }




        public Task<bool> UpdateAsync(ProductSale productSale)
        {
            _context.ProductSales.Update(productSale);
            return Task.FromResult(true);
        }

    }
}
