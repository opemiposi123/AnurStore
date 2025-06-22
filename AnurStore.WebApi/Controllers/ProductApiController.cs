using AnurStore.Application.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;

namespace AnurStore.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductApiController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductApiController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts(string query)
        {
            var products = await _productService.SearchProductsByNameAsync(query);

            var result = products.Select(p => new
            {
                p.Id,
                display = $"{p.Name} ({p.CategoryName}) {p.SizeWithUnit}"
            });

            return Ok(result);
        }
    }
}
