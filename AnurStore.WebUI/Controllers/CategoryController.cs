using AnurStore.Application.Abstractions.Services;
using AnurStore.Application.DTOs;
using AnurStore.Application.RequestModel;
using AspNetCoreHero.ToastNotification.Abstractions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace AnurStore.WebUI.Controllers
{
    public class CategoryController : Controller
    {
        private readonly INotyfService _notyf;
        private readonly ICategoryService _categoryService;
        public CategoryController(INotyfService notyf, ICategoryService categoryService)
        {
            _notyf = notyf;
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index()
        {
            var response = await _categoryService.GetAllCategory();
            if (response.Status)
            {
                return View(response.Data);
            }
            return View(Enumerable.Empty<CategoryDto>());
        }

        public async Task<IActionResult> ViewCategoryDetail(string id)
        {
            var category = await _categoryService.GetCategory(id);

            return category == null
                       ? (IActionResult)NotFound()
                       : View(category.Data);
        }

        public IActionResult CreateCategory() =>
          View();

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequest model, [FromServices] IValidator<CreateCategoryRequest> validator)
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
            var response = await _categoryService.CreateCategory(model);
            _notyf.Success("Category Created Succesfully");
            return RedirectToAction("Index", "Category");
        }
        
        [HttpGet]
        public async Task<IActionResult> DeleteCategory([FromRoute] string id)
        {
            var response = await _categoryService.DeleteCategory(id);
            _notyf.Success("Category Deleted Succesfully");
            return RedirectToAction("Index", "Category");
        }

        public async Task<IActionResult> EditCategory(string id)
        {
            var response = await _categoryService.GetCategory(id);
            if (response == null)
            {
                return NotFound();
            }
            return View(response.Data);
        }

        [HttpPost]
        public async Task<IActionResult> EditCategory([FromRoute] string id, [FromForm] UpdateCategoryRequest model, [FromServices] IValidator<UpdateCategoryRequest> validator)
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
            var response = await _categoryService.UpdateCategory(id, model);
            _notyf.Success("Category Updated Succesfully");
            return RedirectToAction("Index", "Category");
        }
    }
}