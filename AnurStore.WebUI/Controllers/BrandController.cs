using AnurStore.Application.Abstractions.Services;
using AnurStore.Application.RequestModel;
using AspNetCoreHero.ToastNotification.Abstractions;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnurStore.WebUI.Controllers
{
    public class BrandController : Controller
    {
        private readonly INotyfService _notyf;
        private readonly IBrandService _brandService;
        public BrandController(INotyfService notyf, IBrandService brandService)
        {
            _notyf = notyf;
            _brandService = brandService;
        }
        public async Task<IActionResult> Index()
        {
            var result = await _brandService.GetAllBrand();
            return View(result);
        }

        public async Task<IActionResult> ViewBrandDetail(string id)
        {
            var brand = await _brandService.GetBrand(id);

            return brand == null
                       ? (IActionResult)NotFound()
                       : View(brand);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult CreateBrand() =>
          View();

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateBrand(CreateBrandRequest model, [FromServices] IValidator<CreateBrandRequest> validator)
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
            var response = await _brandService.CreateBrand(model);
            _notyf.Success("Brand Created Succesfully");
            return RedirectToAction("Index", "Brand");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteBrand([FromRoute] string id)
        {
            var response = await _brandService.DeleteBrand(id);
            _notyf.Success("Brand Deleted Succesfully");
            return RedirectToAction("Index", "Brand");
        }

        public async Task<IActionResult> EditBrand(string id)
        {
            var instance = await _brandService.GetBrand(id);
            if (instance == null)
            {
                return NotFound();
            }
            return View(instance);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditBrand([FromRoute] string id, [FromForm] UpdateBrandRequest model, [FromServices] IValidator<UpdateBrandRequest> validator)
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
            var response = await _brandService.UpdateBrand(id, model);
            _notyf.Success("Brand Updated Succesfully");
            return RedirectToAction("Index", "Brand");
        }
    }
}
