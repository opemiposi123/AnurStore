using AnurStore.Application.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;

namespace AnurStore.WebUI.Controllers
{
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(DateTime? fromDate, DateTime? toDate, string? paymentType)
        {
            var report = await _reportService.GetSalesReportAsync(fromDate, toDate, paymentType);
            return View(report);
        }
    }
}
