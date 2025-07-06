using AnurStore.Application.Abstractions.Repositories;
using AnurStore.Application.Abstractions.Services;
using AnurStore.Application.DTOs;
using AnurStore.Domain.Enums;
using DinkToPdf;
using DinkToPdf.Contracts;
using System.Text;

namespace AnurStore.Application.Services
{
    public class ReportService : IReportService
    {
        private readonly IProductSaleRepository _saleRepository;
        private readonly IConverter _converter;

        public ReportService(IProductSaleRepository saleRepository, IConverter converter)
        {
            _saleRepository = saleRepository;
            _converter = converter;
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

        public async Task<byte[]> ExportSalesReportToPdfAsync(SalesReportDto report)
        {
            var html = GenerateHtml(report); // build the HTML string from DTO

            var doc = new HtmlToPdfDocument
            {
                GlobalSettings = {
                PaperSize = PaperKind.A4,
                Orientation = Orientation.Portrait,
            },
                Objects = {
                new ObjectSettings {
                    HtmlContent = html,
                    WebSettings = { DefaultEncoding = "utf-8" }
                }
            }
            };

            return _converter.Convert(doc);
        }

        private string GenerateHtml(SalesReportDto report)
        {
            // ⚠️ Simplified HTML here. Use StringBuilder or Razor ViewToStringRenderer.
            var builder = new StringBuilder();
            builder.AppendLine("<h2>Sales Report</h2>");
            builder.AppendLine($"<p><strong>Date:</strong> {DateTime.Now:MMMM yyyy}</p>");
            builder.AppendLine("<table border='1' cellpadding='5'><tr><th>Order ID</th><th>Customer</th><th>Sold By</th><th>Amount</th><th>Payment Type</th><th>Status</th></tr>");

            foreach (var sale in report.Sales)
            {
                builder.AppendLine($"<tr><td>{sale.OrderId}</td><td>{sale.CustomerName}</td><td>{sale.SoldBy}</td><td>₦{sale.Amount:N2}</td><td>{sale.PaymentType}</td><td>{sale.Status}</td></tr>");
            }

            builder.AppendLine("</table>");
            return builder.ToString();
        }

    }

}
