using AnurStore.Application.Abstractions.Repositories;
using AnurStore.Application.Abstractions.Services;
using AnurStore.Application.DTOs;
using AnurStore.Domain.Enums;

namespace AnurStore.Application.Services
{
    public class ReportService : IReportService
    {
        private readonly IProductSaleRepository _saleRepository;

        public ReportService(IProductSaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public async Task<SalesReportDto> GetSalesReportAsync(
            DateTime? fromDate = null, DateTime? toDate = null, string? paymentType = null)
        {
            var sales = await _saleRepository.GetSalesWithFiltersAsync(fromDate, toDate, paymentType);

            var report = new SalesReportDto
            {
                TotalCash = sales.Where(s => s.PaymentMethod == PaymentMethod.Cash).Sum(s => s.TotalAmount),
                TotalPOS = sales.Where(s => s.PaymentMethod == PaymentMethod.POS).Sum(s => s.TotalAmount),
                TotalTransfer = sales.Where(s => s.PaymentMethod == PaymentMethod.BankTransfer).Sum(s => s.TotalAmount),
                TotalCheque = sales.Where(s => s.PaymentMethod == PaymentMethod.Cheque).Sum(s => s.TotalAmount),
                Sales = sales.Select(s => new SaleRecordDto
                {
                    OrderId = $"INV-{s.Id.Substring(0, 8).ToUpper()}",
                    CustomerName = s.CustomerName ?? "Guest",
                    SoldBy = s.CreatedBy ?? "Admin",
                    Amount = s.TotalAmount,
                    PaymentType = s.PaymentMethod.ToString(),
                    Status = "Paid"
                }).ToList()
            };

            return report;
        }

    }

}
