using AnurStore.Application.Abstractions.Repositories;
using AnurStore.Application.Pagination;
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
            return productPurchase;
        }

        public async Task<List<ProductPurchase>> GetAllAsync( string username = null)
        {
            IQueryable<ProductPurchase> query = _context.ProductPurchases
                .Include(p => p.Supplier)
                .Include(p => p.PurchaseItems)
                    .ThenInclude(p => p.Product);

            if (!string.IsNullOrEmpty(username))
            {
                query = query.Where(p => p.CreatedBy == username);
            }

            return await query.ToListAsync();

        }

        public async Task<ProductPurchase?> GetByIdAsync(string id)
        {
            return await _context.ProductPurchases
                .Include(p => p.Supplier)
                .Include(p => p.PurchaseItems)
                  .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IList<ProductPurchase>> GetBySupplierIdAsync(string supplierId)
        {
            return await _context.ProductPurchases
                .Include(p => p.Supplier)
                .Where(p => p.SupplierId == supplierId)
                .ToListAsync();
        }

        public async Task<IList<ProductPurchase>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.ProductPurchases
                .Include(p => p.Supplier)
                .Where(p => p.PurchaseDate >= startDate && p.PurchaseDate <= endDate)
                .ToListAsync();
        }

        public async Task<IList<ProductPurchase>> GetPurchasesByProductAsync(string productId)
        {
            return await _context.ProductPurchaseItems
                .Where(i => i.ProductId == productId)
                .Select(i => i.ProductPurchase)
                .Include(p => p.Supplier)
                .Distinct()
                .ToListAsync();
        }
        public async Task<(IList<ProductPurchase> Purchases, int TotalCount)> GetPagedPurchasesAsync(PurchaseFilterRequest filter)
        {
            var query = _context.ProductPurchases
                .Include(p => p.Supplier)
                .Include(p => p.PurchaseItems)
                    .ThenInclude(i => i.Product)
                .AsQueryable();

            if (filter.StartDate.HasValue)
                query = query.Where(p => p.PurchaseDate >= filter.StartDate.Value);

            if (filter.EndDate.HasValue)
                query = query.Where(p => p.PurchaseDate <= filter.EndDate.Value);

            if (!string.IsNullOrEmpty(filter.SupplierId))
                query = query.Where(p => p.SupplierId == filter.SupplierId);

            if (!string.IsNullOrEmpty(filter.ProductId))
                query = query.Where(p => p.PurchaseItems.Any(i => i.ProductId == filter.ProductId));

            var totalCount = await query.CountAsync();

            var pagedPurchases = await query
                .OrderByDescending(p => p.PurchaseDate)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            return (pagedPurchases, totalCount);
        }

        public async Task<IList<ProductPurchase>> GetAllWithDetailsAsync()
        {
            return await _context.ProductPurchases
                .Include(p => p.Supplier)
                .Include(p => p.PurchaseItems)
                    .ThenInclude(i => i.Product)
                .ToListAsync();
        }


        public async Task<bool> UpdateAsync(ProductPurchase productPurchase)
        {
            var existing = await _context.ProductPurchases.FindAsync(productPurchase.Id);
            if (existing == null)
                return false;

            _context.Entry(existing).CurrentValues.SetValues(productPurchase);
            return true;
        }
    }

}
