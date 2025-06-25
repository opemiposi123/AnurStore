using AnurStore.Application.Abstractions.Services;
using AnurStore.Application.DTOs;
using AnurStore.Application.RequestModel;
using AnurStore.Application.Services;
using AnurStore.Domain.Entities;
using AspNetCoreHero.ToastNotification.Abstractions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Channels;

namespace AnurStore.WebUI.Controllers
{
    [Route("[controller]")]
    public class ProductSaleController : Controller
    {
        private readonly IProductSaleService _productSaleService;
        private readonly IProductService _productService;
        private readonly INotyfService _notyf;

        public ProductSaleController(IProductSaleService productSaleService, IProductService productService, INotyfService notyf)
        {
            _productSaleService = productSaleService;
            _productService = productService;
            _notyf = notyf;
        }

        [HttpGet("get-product-sales")]
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
        {
            var response = await _productSaleService.GetAllProductSalesPagedAsync(pageNumber, pageSize);

            var paginatedList = new PaginatedList<ProductSaleDto>(
                response.Data, response.TotalRecords, pageNumber, pageSize
            );
            return View(paginatedList);
        }

        [HttpGet("create-product-sale")]
        public async Task<IActionResult> CreateProductSale()
        {
            var productsResponse = await _productSaleService.GetTopFrequentlySoldProductsAsync(7);

            List<ProductDto> products;

            if (productsResponse.Status && productsResponse.Data != null && productsResponse.Data.Any())
            {
                // If there are recent sales, use frequently sold products
                products = productsResponse.Data.ToList();
            }
            else
            {
                // If no sales yet, fallback to all products
                var allProducts = await _productService.GetAllDisplayProducts();
                products = allProducts.Data?.ToList() ?? new List<ProductDto>();
            }

            var viewModel = new CreateProductSaleViewModel
            {
                SaleRequest = new CreateProductSaleRequest(),
                AvailableProducts = products
            };

            return View(viewModel);
        }



        [HttpGet("search-products")]
        public async Task<IActionResult> SearchProducts(string term)
        {
            var products = await _productService.SearchProductsAsync(term);

            var result = products.Select(p => new
            {
                id = p.Id,
                text = p.Name
            });

            return Json(result);
        }




        [HttpPost("create-product-sale")]
        public async Task<IActionResult> CreateProductSale(string SaleRequestJson)
        {
            var request = JsonConvert.DeserializeObject<CreateProductSaleRequest>(SaleRequestJson);

            if (request == null)
            {
                _notyf.Error("Invalid request data.");
                return RedirectToAction("CreateProductSale");
            }

            var response = await _productSaleService.AddProductSale(request);

            if (!response.Status)
            {
                _notyf.Error(response.Message);
                return RedirectToAction("CreateProductSale");
            }

            _notyf.Success("Product Sale Created Successfully");
            return File(response.Data, "application/pdf", "ProductSaleReceipt.pdf");

        }




        [HttpGet("product-sale-cancel/{id}")]
        public async Task<IActionResult> CancelProductSaleAsync([FromRoute] string id)
        {
            var response = await _productSaleService.CancelProductSaleAsync(id);
            _notyf.Success("Product sale canceled successfully.");
            return RedirectToAction("Index", "Product Sale");
        }

        [HttpGet("product-sale/edit/{id}")]
        public async Task<IActionResult> EditProductSale(string id)
        {
            var response = await _productSaleService.GetProductSaleById(id);
            if (!response.Status)
            {
                _notyf.Error(response.Message);
                return NotFound();
            }
            _notyf.Error(response.Message);
            return View(response.Data);
        }

        [HttpPost("update-product-sale")]
        public async Task<IActionResult> EditProductSale([FromRoute] string id, [FromForm] UpdateProductSaleRequest request)
        {var response = await _productSaleService.UpdateProductSaleAsync(request.Id, request);
            if (response.Status)
            {
                _notyf.Success("Product Updated Succesfully");
                return RedirectToAction("Index", "Product");
            }
            _notyf.Error(response.Message, 3);
            return View(request);
        }


        [HttpGet("get-sale-by-id")]
        public async Task<IActionResult> ViewProductSaleDetail(string id)
        {
            var response = await _productSaleService.GetProductSaleById(id);

            if (response != null && response.Status)
            {
                _notyf.Success(response.Message);
                return View(response.Data);
            }

            _notyf.Error(response?.Message ?? "Failed to load sale details");
            return RedirectToAction("Index");
        }


        [HttpGet("get-product-by-id")]
        public async Task<IActionResult> GetProductById(string id)
        {
            var response = await _productService.GetProductDetails(id);

            if (response != null && response.Status)
            {
                _notyf.Success(response.Message);
                return Json(response.Data); 
            }

            _notyf.Error(response?.Message ?? "Failed to load sale details");
            return RedirectToAction("Index");
        }


        [HttpGet("product-sale/filter")]
        public async Task<IActionResult> FilterSales([FromQuery] ProductSaleFilterRequest filter)
        {
            if (filter.PageNumber <= 0) filter.PageNumber = 1;
            if (filter.PageSize <= 0) filter.PageSize = 10;
            var response = await _productSaleService.GetFilteredProductSalesPagedAsync(filter);
            if (response != null)
            {
                _notyf.Success(response.Message);
                return RedirectToAction("Index");
            }
            ViewBag.Filter = filter;
            return View("Index", response);
        }

    }
}
