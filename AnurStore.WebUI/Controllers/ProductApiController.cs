using AnurStore.Application.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;

namespace AnurStore.WebUI.Controllers;

[ApiController]
[Route("api/product")]
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
        try
        { 
            var products = await _productService.SearchProductsByNameAsync(query);

            if (products == null || !products.Any())
            {
                return Ok(Array.Empty<object>());
            }

            var result = products.Select(p => new
            {
                p.Id,
                Display = $"{p.Name} ({p.CategoryName ?? "N/A"}) {p.SizeWithUnit ?? ""}" 
            });

            return Ok(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine("EXCEPTION: " + ex.Message);
            return StatusCode(500, $"Internal server error: {ex.Message}");
        } 
    }
}

