using AnurStore.Application.Abstractions.Services;
using AnurStore.Application.DTOs;
using AnurStore.Application.RequestModel;
using AspNetCoreHero.ToastNotification.Abstractions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace AnurStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private readonly INotyfService _notyf; 
        private readonly IProductService  _productService;
        private readonly IProductUnitService  _productUnitService;
        private readonly ICategoryService  _categoryService; 
        private readonly IBrandService  _brandService;  
        public ProductController(INotyfService notyf, 
            IProductService productService, 
            IProductUnitService productUnitService, 
            ICategoryService categoryService, 
            IBrandService brandService)
        {
            _notyf = notyf;
            _productService = productService;
            _productUnitService = productUnitService;
            _categoryService = categoryService;
            _brandService = brandService;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _productService.GetAllProduct();
            if (response.Status)
            {
                return View(response.Data);
            }
            return View(Enumerable.Empty<ProductDto >());
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts(string query)
        {
            var products = await _productService.SearchProductsByNameAsync(query);

            var result = products.Select(p => new
            {
                p.Id,
                Display = $"{p.Name} ({p.CategoryName}) {p.SizeWithUnit}" 
            });

            return Ok(result);
        }


        public async Task<IActionResult> ViewProductDetail(string id)
        {
            var product = await _productService.GetProductDetails(id);

            return product == null
                       ? (IActionResult)NotFound()
                       : View(product.Data);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteProduct([FromRoute] string id)
        {
            var response = await _productService.DeleteProduct(id);
            _notyf.Success("Product Deleted Succesfully");
            return RedirectToAction("Index", "Product"); 
        }

        public async Task<IActionResult> CreateProduct()
        {
            ViewBag.Categories = await _categoryService.GetCategorySelectList();
            ViewBag.Brands = await _brandService.GetBrandSelectList();
            ViewBag.ProductUnits = await _productUnitService.GetProductUnitSelectList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductRequest model, [FromServices] IValidator<CreateProductRequest> validator)
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
            var response = await _productService.CreateProductAsync(model);
            _notyf.Success("Product Created Succesfully");
            return RedirectToAction("Index", "Product");
        }

        public async Task<IActionResult> EditProduct(string id)
        {
            ViewBag.Categories = await _categoryService.GetCategorySelectList();
            ViewBag.Brands = await _brandService.GetBrandSelectList();
            ViewBag.ProductUnits = await _productUnitService.GetProductUnitSelectList();
            var response = await _productService.GetProductDetails(id);
            if (response == null)
            {
                return NotFound();
            }
            return View(response.Data);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct([FromRoute] string id, [FromForm] UpdateProductRequest model, [FromServices] IValidator<UpdateProductRequest> validator)
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
            var response = await _productService.UpdateProduct(id, model);
            _notyf.Success("Product Updated Succesfully");
            return RedirectToAction("Index", "Product");
        }

        [HttpGet]
        public IActionResult UploadProductsFromExcel() => View(); 

        [HttpPost]
        public async Task<IActionResult> UploadProductsFromExcel(IFormFile excelFile)
        {
            if (excelFile == null || excelFile.Length == 0)
            {
                _notyf.Error("Please select a valid Excel file.");
                return RedirectToAction("Index");
            }

            try
            {
                using var stream = excelFile.OpenReadStream();
                await _productService.UploadProductsFromExcelAsync(stream);
                _notyf.Success("Products uploaded successfully.");
            }
            catch (ApplicationException ex)
            {
                _notyf.Error(ex.Message);
            }
            catch (Exception ex)
            {
                _notyf.Error("An unexpected error occurred while uploading products. Please try again later.");
                Console.WriteLine(ex.Message); 
            }

            return RedirectToAction("Index");
        } 

        [HttpGet]
        public async Task<IActionResult> DownloadProductTemplate()
        {
            try
            {
                var fileResult = await _productService.DownloadProductTemplateAsync();
                return fileResult;
            }
            catch (Exception ex)
            {
                _notyf.Error("An error occurred while generating the product template. Please try again.");
                Console.WriteLine(ex.Message); 
                return RedirectToAction("Index");
            }
        }

    }
}
 