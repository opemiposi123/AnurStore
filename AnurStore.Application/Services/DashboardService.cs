using AnurStore.Application.Abstractions.Repositories;
using AnurStore.Application.Abstractions.Services;
using AnurStore.Application.DTOs;

namespace AnurStore.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepo;

        public DashboardService(IDashboardRepository dashboardRepo)
        {
            _dashboardRepo = dashboardRepo;
        }

        public async Task<DashboardCountDto> GetDashboardDataAsync()
        {
            var currentYear = DateTime.Now.Year;

            return new DashboardCountDto
            {
                TotalProducts = await _dashboardRepo.GetTotalProductsAsync(),
                TotalPurchases = await _dashboardRepo.GetTotalPurchasesAsync(),
                TotalSales = await _dashboardRepo.GetTotalSalesAsync(),
                TotalCategories = await _dashboardRepo.GetTotalCategoriesAsync(),
                TotalRevenue = await _dashboardRepo.GetTotalRevenueAsync(),
                TopProducts = await _dashboardRepo.GetTopProductsAsync(),
                BestSellingProducts = await _dashboardRepo.GetBestSellingProductsAsync(),
                RevenueCostData = await _dashboardRepo.GetMonthlyRevenueCostAsync(currentYear),
                DailySalesData = await _dashboardRepo.GetDailySalesAsync()
            };
        }
    }

}
