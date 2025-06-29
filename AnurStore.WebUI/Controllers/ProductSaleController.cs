using AnurStore.Application.Abstractions.Services;
using AnurStore.Application.DTOs;
using AnurStore.Application.RequestModel;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
                products = productsResponse.Data.ToList();
            }
            else
            {
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

        //[HttpPost("create-product-sale")]
        //public async Task<IActionResult> CreateProductSale(string SaleRequestJson)
        //{
        //    var request = JsonConvert.DeserializeObject<CreateProductSaleRequest>(SaleRequestJson);

        //    if (request == null)
        //    {
        //        _notyf.Error("Invalid request data.");
        //        return RedirectToAction("CreateProductSale");
        //    }

        //    var response = await _productSaleService.AddProductSale(request);

        //    if (!response.Status)
        //    {
        //        _notyf.Error(response.Message);
        //        return RedirectToAction("CreateProductSale");
        //    }

        //    _notyf.Success("Product Sale Created Successfully");
        //    return File(response.Data, "application/pdf", "ProductSaleReceipt.pdf");

        //}

        [HttpPost("create-product-sale")]
        public async Task<IActionResult> CreateProductSale(string SaleRequestJson)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SaleRequestJson))
                {
                    _notyf.Error("Invalid request data.");
                    return RedirectToAction("CreateProductSale");
                }

                CreateProductSaleRequest request;
                try
                {
                    request = JsonConvert.DeserializeObject<CreateProductSaleRequest>(SaleRequestJson);
                }
                catch (JsonException)
                {
                    _notyf.Error("Invalid JSON data format.");
                    return RedirectToAction("CreateProductSale");
                }

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
                var receiptId = Guid.NewGuid().ToString();
                var tempPath = Path.GetTempPath();
                var filePath = Path.Combine(tempPath, $"receipt_{receiptId}.pdf");

                try
                {
                    await System.IO.File.WriteAllBytesAsync(filePath, response.Data);

                    TempData["ReceiptFilePath"] = filePath;
                    TempData["ReceiptId"] = receiptId;
                    TempData["ReceiptFileName"] = $"Receipt_{DateTime.Now:yyyyMMddHHmmss}.pdf";

                    _notyf.Success("Product Sale Created Successfully");

                    return RedirectToAction("DisplayReceipt");
                }
                catch 
                {
                    _notyf.Error("Failed to save receipt file.");
                    return RedirectToAction("CreateProductSale");
                }
            }
            catch 
            {
                _notyf.Error("An error occurred while processing the sale. Please try again.");
                return RedirectToAction("CreateProductSale");
            }
        }

        [HttpGet("display-receipt")]
        public async Task<IActionResult> DisplayReceipt()
        {
            var filePath = TempData["ReceiptFilePath"]?.ToString();
            var receiptId = TempData["ReceiptId"]?.ToString();
            var fileName = TempData["ReceiptFileName"]?.ToString();

            if (string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(filePath))
            {
                _notyf.Warning("No receipt to display or file has expired.");
                return RedirectToAction("Index", "ProductSale");
            }

            try
            {
                var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                var base64String = Convert.ToBase64String(fileBytes);

                TempData.Keep("ReceiptFilePath");
                TempData.Keep("ReceiptId");
                TempData.Keep("ReceiptFileName");

                ViewBag.ReceiptData = base64String;
                ViewBag.ReceiptFileName = fileName ?? "Receipt.pdf";

                return View();
            }
            catch
            {
                _notyf.Error("Failed to load receipt.");
                return RedirectToAction("Index", "ProductSale");
            }
        }





        [HttpGet("download-receipt")]
        public async Task<IActionResult> DownloadReceipt()
        {
            var filePath = TempData["ReceiptFilePath"]?.ToString();
            var fileName = TempData["ReceiptFileName"]?.ToString() ?? "Receipt.pdf";

            if (string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(filePath))
            {
                _notyf.Warning("No receipt available for download or file has expired.");
                return RedirectToAction("Index", "ProductSale");
            }

            try
            {
                var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);

                try
                {
                    System.IO.File.Delete(filePath);
                }
                catch 
                {
                    _notyf.Error("Failed to download receipt.");
                    return RedirectToAction("Index");
                }

                return File(fileBytes, "application/pdf", fileName);
            }
            catch 
            {
                _notyf.Error("Failed to download receipt.");
                return RedirectToAction("Index", "ProductSale");
            }
        }




        [HttpPost("continue-to-sales")]
        public IActionResult ContinueToSales()
        {
            var filePath = TempData["ReceiptFilePath"]?.ToString();
            if (!string.IsNullOrEmpty(filePath) && System.IO.File.Exists(filePath))
            {
                try
                {
                    System.IO.File.Delete(filePath);
                }
                catch
                {
                    _notyf.Error("Failed to download receipt.");
                    return RedirectToAction("Index");
                }
            }
            TempData.Clear();
            return RedirectToAction("Index", "ProductSale");
        }


        [HttpGet("product-sale-cancel/{id}")]
        public async Task<IActionResult> CancelProductSale([FromRoute] string id)
        {
            var response = await _productSaleService.CancelProductSaleAsync(id);
            _notyf.Success("Product sale canceled successfully.");
            return RedirectToAction("Index", "ProductSale");
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
