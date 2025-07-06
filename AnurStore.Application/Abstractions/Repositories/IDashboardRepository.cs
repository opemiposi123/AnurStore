using AnurStore.Application.DTOs;

namespace AnurStore.Application.Abstractions.Repositories
{
    public interface IDashboardRepository
    {
        Task<int> GetTotalProductsAsync();
        Task<int> GetTotalPurchasesAsync();
        Task<int> GetTotalSalesAsync();
        Task<int> GetTotalCategoriesAsync();
        Task<decimal> GetTotalRevenueAsync();

        Task<List<TopProductDto>> GetTopProductsAsync(int count = 3);
        Task<List<BestSellingProductDto>> GetBestSellingProductsAsync(int count = 2);
        Task<List<MonthlyRevenueCostDto>> GetMonthlyRevenueCostAsync(int year);
        Task<List<DailySalesDto>> GetDailySalesAsync(int days = 7);
    }
}
