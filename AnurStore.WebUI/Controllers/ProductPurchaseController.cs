﻿using AnurStore.Application.Abstractions.Services;
using AnurStore.Application.DTOs;
using AnurStore.Application.Pagination;
using AnurStore.Application.RequestModel;
using AnurStore.Application.Services;
using AnurStore.Persistence;
using AspNetCoreHero.ToastNotification.Abstractions;
using Azure;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace AnurStore.WebUI.Controllers
{
    public class ProductPurchaseController : Controller
    {
        private readonly INotyfService _notyf;
        private readonly IProductPurchaseService _productPurchaseService;
        private readonly ISupplierService _supplierService;
        private readonly BatchHelper _batchHelper;
        public ProductPurchaseController(INotyfService notyf,
            IProductPurchaseService productPurchaseService,
            ISupplierService supplierService,
            BatchHelper batchHelper)
        {
            _notyf = notyf;
            _productPurchaseService = productPurchaseService;
            _supplierService = supplierService;
            _batchHelper = batchHelper;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _productPurchaseService.GetAllPurchasesAsync();
            if (response.Status)
            {
                ViewBag.Suppliers = await _supplierService.GetSupplierSelectList();
                return View(response.Data);
            }
            return View(Enumerable.Empty<ProductPurchaseDto>());
        }


        public async Task<IActionResult> CreateProductPurchase()
        {
            var batchNumber = await _batchHelper.GenerateBatchNumberAsync();
            var model = new CreateProductPurchaseRequest
            {
                PurchaseDate = DateTime.Now,
                Batch = batchNumber
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

            var request = await _productPurchaseService.PurchaseProductsAsync(model, userName);
            if (!request.Status)
            {
                _notyf.Error(request.Message);
                ViewBag.Suppliers = await _supplierService.GetSupplierSelectList();
                return View(model);
            }
            _notyf.Success("Product purchased successfully");
            return RedirectToAction("Index", "ProductPurchase");
        }

        [HttpPost]
        public async Task<IActionResult> ProcessInventory(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                _notyf.Success("Invalid purchase ID.");
                return RedirectToAction("Index");
            }
            var userName = User.Identity?.Name ?? "System";
            var result = await _productPurchaseService.ProcessInventoryAndProductUpdateAsync(id, userName);

            if (!result.Status)
            {
                _notyf.Error(result.Message);
            }
            _notyf.Success("Product purchased proccesed successfully");
            return RedirectToAction("Index", "ProductPurchase");
        }

        public async Task<IActionResult> ViewPurchasesBySupplier(string supplierId)
        {
            var response = await _productPurchaseService.GetPurchasesBySupplierAsync(supplierId);
            if (response.Status)
            {
                ViewBag.Suppliers = await _supplierService.GetSupplierSelectList();
                return View("Index", response.Data);
            }
            return View("Index", Enumerable.Empty<ProductPurchaseDto>());
        }

        public async Task<IActionResult> GetPurchasesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var response = await _productPurchaseService.GetPurchasesByDateRangeAsync(startDate, endDate);
            if (response.Status)
            {
                ViewBag.Suppliers = await _supplierService.GetSupplierSelectList();
                return View("Index", response.Data);
            }
            return View("Index", Enumerable.Empty<ProductPurchaseDto>());
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

        public async Task<IActionResult> ViewProductPurchaseDetail([FromRoute] string id)
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
