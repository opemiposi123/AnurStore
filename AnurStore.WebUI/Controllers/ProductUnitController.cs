using AnurStore.Application.Abstractions.Services;
using AnurStore.Application.DTOs;
using AnurStore.Application.RequestModel;
using AspNetCoreHero.ToastNotification.Abstractions;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnurStore.WebUI.Controllers
{
    public class ProductUnitController : Controller
    {
        private readonly INotyfService _notyf;
        private readonly IProductUnitService _productUnitService;
        public ProductUnitController(INotyfService notyf, IProductUnitService productUnitService)
        {
            _notyf = notyf;
            _productUnitService = productUnitService; 
        }
        public async Task<IActionResult> Index()
        {
            var response = await _productUnitService.GetAllProductUnit();
            if (response.Status)
            {
                return View(response.Data);
            }
            return View(Enumerable.Empty<ProductUnitDto>());
        }

        public async Task<IActionResult> ViewProductUnitDetail(string id)
        {
            var ProductUnit = await _productUnitService.GetProductUnit(id);

            return ProductUnit == null
                       ? (IActionResult)NotFound()
                       : View(ProductUnit);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult CreateProductUnit() =>
        View();

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProductUnit(CreateProductUnitRequest model, [FromServices] IValidator<CreateProductUnitRequest> validator)
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(model);
            }
            var response = await _productUnitService.CreateProductUnit(model);
            _notyf.Success("ProductUnit Created Succesfully");
            return RedirectToAction("Index", "ProductUnit");
        }

        // [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> DeleteProductUnit([FromRoute] string id)
        {
            var response = await _productUnitService.DeleteProductUnit(id);
            _notyf.Success("ProductUnit Deleted Succesfully");
            return RedirectToAction("Index", "ProductUnit");
        }

        public async Task<IActionResult> EditProductUnit(string id)
        {
            var result = await _productUnitService.GetProductUnit(id);
            if (result == null)
            {
                return NotFound();
            }
            return View(result.Data);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditProductUnit([FromRoute] string id, [FromForm] UpdateProductUnitRequest model, [FromServices] IValidator<UpdateProductUnitRequest> validator)
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(model);
            }
            var response = await _productUnitService.UpdateProductUnit(id, model);
            _notyf.Success("ProductUnit Updated Succesfully");
            return RedirectToAction("Index", "ProductUnit");
        }
    }
}
