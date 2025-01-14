using AnurStore.Application.Abstractions.Services;
using AnurStore.Application.DTOs;
using AnurStore.Application.Services;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace AnurStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private readonly INotyfService _notyf; 
        private readonly IProductService  _productService;
        public ProductController(INotyfService notyf, IProductService productService)
        {
            _notyf = notyf;
            _productService = productService;
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
    }
}
