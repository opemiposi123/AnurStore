using AnurStore.Application.Abstractions.Services;
using AnurStore.Application.Pagination;
using AnurStore.Application.RequestModel;
using AnurStore.Application.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnurStore.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductPurchaseController : ControllerBase
    {
        private readonly IProductPurchaseService _productPurchaseService;
        private readonly IProductService _productService;

        public ProductPurchaseController(IProductPurchaseService productPurchaseService, IProductService productService)
        {
            _productPurchaseService = productPurchaseService;
            _productService = productService;
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

        [HttpPost("purchase-products")]
        public async Task<IActionResult> CreateProductPurchase([FromBody] CreateProductPurchaseRequest model, [FromServices] IValidator<CreateProductPurchaseRequest> validator)
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new { Field = e.PropertyName, Message = e.ErrorMessage }).ToList();
                return BadRequest(new
                {
                    Message = "Validation failed",
                    Errors = errors
                });
            }

            var userName = User?.Identity?.Name ?? "System";

            await _productPurchaseService.PurchaseProductsAsync(model, userName);

            return Ok(new
            {
                Status = true,
                Message = "Product purchased successfully"
            });
        }




    }
}
