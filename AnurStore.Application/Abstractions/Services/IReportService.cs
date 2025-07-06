using AnurStore.Application.DTOs;

namespace AnurStore.Application.Abstractions.Services
{
    public interface IReportService
    {
        Task<SalesReportDto> GetSalesReportAsync(DateTime? fromDate, DateTime? toDate, string? paymentType);
        Task<byte[]> ExportSalesReportToPdfAsync(SalesReportDto report);
    }
}
