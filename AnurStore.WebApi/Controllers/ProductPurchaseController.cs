using AnurStore.Application.Abstractions.Services;
using AnurStore.Application.Pagination;
using AnurStore.Application.RequestModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnurStore.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductPurchaseController : ControllerBase
    { 
        private readonly IProductPurchaseService _productPurchaseService;
        public ProductPurchaseController(IProductPurchaseService productPurchaseService)
        {
            _productPurchaseService = productPurchaseService; 
        }
        [HttpPost("export")]
        public async Task<IActionResult> ExportPurchases([FromBody] PurchaseExportRequest request)
        {
            var result = await _productPurchaseService.ExportPurchasesToExcelAsync(request);
            if (!result.Status || result.Data == null)
                return BadRequest(result.Message);

            return File(result.Data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "purchases.xlsx");
        }

        [HttpPost("paged")]
        public async Task<IActionResult> GetPagedPurchases([FromBody] PurchaseFilterRequest filter)
        {
            var result = await _productPurchaseService.GetPurchasesPagedAsync(filter);
            return Ok(result);
        }


    }
}
