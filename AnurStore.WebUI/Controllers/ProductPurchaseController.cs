using AnurStore.Application.Abstractions.Services;
using AnurStore.Application.DTOs;
using AnurStore.Application.Pagination;
using AnurStore.Application.RequestModel;
using AnurStore.Application.Services;
using AspNetCoreHero.ToastNotification.Abstractions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace AnurStore.WebUI.Controllers
{
    public class ProductPurchaseController : Controller
    {
        private readonly INotyfService _notyf;
        private readonly IProductPurchaseService _productPurchaseService;
        private readonly ISupplierService _supplierService;
        public ProductPurchaseController(INotyfService notyf, IProductPurchaseService productPurchaseService, ISupplierService supplierService)
        {
            _notyf = notyf;
            _productPurchaseService = productPurchaseService;
            _supplierService = supplierService;
        }

        public async Task<IActionResult> CreateProductPurchase()
        {
            var model = new CreateProductPurchaseRequest
            {
                PurchaseDate = DateTime.Now
            };

            ViewBag.Suppliers = await _supplierService.GetSupplierSelectList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductPurchase(CreateProductPurchaseRequest model, [FromServices] IValidator<CreateProductPurchaseRequest> validator)
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                    _notyf.Error(error.ErrorMessage); 
                }

                ViewBag.Suppliers = await _supplierService.GetSupplierSelectList();
                return View(model);
            }
            var userName = User?.Identity?.Name ?? "System";

            await _productPurchaseService.PurchaseProductsAsync(model,userName);
            _notyf.Success("Product purchased successfully");
            return RedirectToAction("Index", "ProductPurchase");
        }

        public async Task<IActionResult> Index()
        {
            var response = await _productPurchaseService.GetAllPurchasesAsync();
            if (response.Status)
            {
                return View(response.Data);
            }
            return View(Enumerable.Empty<ProductPurchaseDto>());
        } 
        
        public async Task<IActionResult> ViewPurchasesBySupplier(string supplierId)  
        {
            var response = await _productPurchaseService.GetPurchasesBySupplierAsync(supplierId);
            if (response.Status)
            {
                return View(response.Data);
            }
            return View(Enumerable.Empty<ProductPurchaseDto>());
        } 

        public async Task<IActionResult> ViewPurchasesByProductAsync(string productId)   
        {
            var response = await _productPurchaseService.GetPurchasesBySupplierAsync(productId); 
            if (response.Status)
            {
                return View(response.Data);
            }
            return View(Enumerable.Empty<ProductPurchaseDto>());
        } 

        public async Task<IActionResult> GetPurchasesByDateRangeAsync(DateTime startDate, DateTime endDate)   
        {
            var response = await _productPurchaseService.GetPurchasesByDateRangeAsync(startDate, endDate);
            if (response.Status)
            {
                return View(response.Data);
            }
            return View(Enumerable.Empty<ProductPurchaseDto>());
        }

        public async Task<IActionResult> ViewProductDetail(string id)
        {
            var productPurchase = await _productPurchaseService.GetPurchaseDetailsAsync(id);

            return productPurchase == null
                       ? (IActionResult)NotFound()
                       : View(productPurchase.Data);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteProduct([FromRoute] string id)
        {
            var response = await _productPurchaseService.DeletePurchaseAsync(id);
            _notyf.Success("Product purchase Deleted Succesfully");
            return RedirectToAction("Index", "ProductPurchase");
        }

        [HttpGet]
        public async Task<IActionResult> ViewAllPurchases(PurchaseFilterRequest filter) 
        {
            var response = await _productPurchaseService.GetPurchasesPagedAsync(filter);

            if (!response.Status)
            {
                TempData["Error"] = response.Message;
                return View(new PaginatedResult<ProductPurchaseDto>
                {
                    Items = [],
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    TotalCount = 0
                });
            }

            return View(response.Data);
        }
    }
}
