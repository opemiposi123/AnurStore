using AnurStore.Application.Abstractions.Repositories;
using AnurStore.Application.Abstractions.Services;
using AnurStore.Application.RequestModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnurStore.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptController : ControllerBase
    {
        private readonly IProductSaleService _productSaleService;

        public ReceiptController(IReceiptService receiptService , IProductSaleService productSaleService)
        {
           _productSaleService = productSaleService;
        }

        [HttpPost("sales")]
        public async Task<IActionResult> CreateSale([FromBody] CreateProductSaleRequest request)
        {
            var result = await _productSaleService.AddProductSale(request);

            if (!result.Status || result.Data == null)
                return BadRequest(result.Message);

            // This sets the response to trigger a file download in the browser
            return File(result.Data, "application/pdf", $"Receipt_{DateTime.Now:yyyyMMddHHmmss}.pdf");
        }


    }
}
