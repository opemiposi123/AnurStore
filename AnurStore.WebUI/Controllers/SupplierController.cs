using AnurStore.Application.Abstractions.Services;
using AnurStore.Application.DTOs;
using AnurStore.Application.RequestModel;
using AspNetCoreHero.ToastNotification.Abstractions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace AnurStore.WebUI.Controllers
{
    public class SupplierController : Controller
    {
        private readonly INotyfService _notyf;
        private readonly ISupplierService _supplierService;
        public SupplierController(INotyfService notyf, ISupplierService supplierService)
        {
            _notyf = notyf;
            _supplierService = supplierService;
        }
        public async Task<IActionResult> Index()
        {
            var response = await _supplierService.GetAllSupplier();
            if (response.Status)
            {
                return View(response.Data);
            }
            return View(Enumerable.Empty<SupplierDto>()); 
        }

        public async Task<IActionResult> ViewSupplierDetail(string id)
        {
            var supplier = await _supplierService.GetSupplier(id);

            return supplier == null
                       ? (IActionResult)NotFound()
                       : View(supplier);
        }
         
        public IActionResult CreateSupplier() =>
          View();

        [HttpPost]
        public async Task<IActionResult> CreateSupplier(CreateSupplierRequest model, [FromServices] IValidator<CreateSupplierRequest> validator)
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
            var response = await _supplierService.CreateSupplier(model);
            _notyf.Success("Supplier Created Succesfully");
            return RedirectToAction("Index", "Supplier");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteSupplier([FromRoute] string id)
        {
            var response = await _supplierService.DeleteSupplier(id);
            _notyf.Success("Supplier Deleted Succesfully");
            return RedirectToAction("Index", "Supplier");
        }

        public async Task<IActionResult> EditSupplier(string id)
        {
            var response = await _supplierService.GetSupplier(id);
            if (response == null)
            {
                return NotFound();
            }
            return View(response.Data);
        }

        [HttpPost]
        public async Task<IActionResult> EditSupplier([FromRoute] string id, [FromForm] UpdateSupplierRequest model, [FromServices] IValidator<UpdateSupplierRequest> validator)
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
            var response = await _supplierService.UpdateSupplier(id, model);
            _notyf.Success("Supplier Updated Succesfully");
            return RedirectToAction("Index", "Supplier");
        }
    }
}
