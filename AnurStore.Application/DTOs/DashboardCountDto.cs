namespace AnurStore.Application.DTOs
{
    public class DashboardCountDto
    {
        public int TotalProducts { get; set; }
        public int TotalPurchases { get; set; }
        public int TotalSales { get; set; }
        public int TotalCategories { get; set; }
        public decimal TotalRevenue { get; set; }

        public List<TopProductDto> TopProducts { get; set; }
        public List<BestSellingProductDto> BestSellingProducts { get; set; }

        public List<MonthlyRevenueCostDto> RevenueCostData { get; set; }
        public List<DailySalesDto> DailySalesData { get; set; }
    }

    public class TopProductDto
    {
        public string ProductName { get; set; }
        public int QuantityInStock { get; set; }
        public string ImageUrl { get; set; }
    }

    public class BestSellingProductDto
    {
        public string ProductName { get; set; }
        public int TotalSold { get; set; }
        public decimal TotalEarned { get; set; }
        public string ImageUrl { get; set; }
    }

    public class MonthlyRevenueCostDto
    {
        public string Month { get; set; } 
        public decimal Revenue { get; set; }
        public decimal Cost { get; set; }
    }

    public class DailySalesDto
    {
        public DateTime Date { get; set; }
        public decimal SalesAmount { get; set; }
    }

}
