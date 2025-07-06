using AnurStore.Application.Abstractions.Repositories;
using AnurStore.Application.DTOs;
using AnurStore.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace AnurStore.Persistence.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationContext _context;

        public DashboardRepository(ApplicationContext context)
        {
            _context = context;
        }

        public Task<int> GetTotalProductsAsync() =>
            _context.Products.CountAsync();

        public Task<int> GetTotalPurchasesAsync() =>
            _context.ProductPurchases.CountAsync();

        public Task<int> GetTotalSalesAsync() =>
            _context.ProductSales.CountAsync();

        public Task<int> GetTotalCategoriesAsync() =>
            _context.Categories.CountAsync();

        public Task<decimal> GetTotalRevenueAsync() =>
            _context.ProductSales.SumAsync(s => s.TotalAmount);

        public Task<List<TopProductDto>> GetTopProductsAsync(int count = 3) =>
      _context.Inventories
          .Include(i => i.Product)
          .OrderByDescending(i => i.TotalPiecesAvailable)
          .Take(count)
          .Select(i => new TopProductDto
          {
              ProductName = i.Product.Name,
              QuantityInStock = i.TotalPiecesAvailable,
              ImageUrl = i.Product.ProductImageUrl
          }).ToListAsync();


        public Task<List<BestSellingProductDto>> GetBestSellingProductsAsync(int count = 2) =>
          _context.ProductSaleItems
              .Include(p => p.Product) 
              .GroupBy(s => new { s.ProductId, s.Product.Name })
              .Select(g => new BestSellingProductDto
              {
                  ProductName = g.Key.Name,
                  TotalSold = g.Sum(x => x.Quantity),
                  TotalEarned = g.Sum(x => x.SubTotal),
                  ImageUrl = g.First().Product.ProductImageUrl!
              })
              .OrderByDescending(x => x.TotalSold)
              .Take(count)
              .ToListAsync();

        public Task<List<MonthlyRevenueCostDto>> GetMonthlyRevenueCostAsync(int year) =>
            _context.ProductSales
                .Where(s => s.SaleDate.Year == year)
                .GroupBy(s => s.SaleDate.Month)
                .Select(g => new MonthlyRevenueCostDto
                {
                    Month = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(g.Key),
                    Revenue = g.Sum(x => x.TotalAmount),
                    Cost = 0
                }).ToListAsync();

        public Task<List<DailySalesDto>> GetDailySalesAsync(int days = 7)
        {
            var today = DateTime.Today;
            return _context.ProductSales
                .Where(s => s.SaleDate.Date >= today.AddDays(-days))
                .GroupBy(s => s.SaleDate.Date)
                .Select(g => new DailySalesDto
                {
                    Date = g.Key,
                    SalesAmount = g.Sum(x => x.TotalAmount)
                }).ToListAsync();
        }
    }

}
