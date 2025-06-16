using AnurStore.Application.Abstractions.Services;
using AnurStore.Application.DTOs;
using AnurStore.Application.RequestModel;
using AnurStore.Application.Wrapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSwag.Annotations;

namespace AnurStore.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IBrandService _brandService;
        private readonly IProductUnitService _productUnitService;

        public ProductController(IProductService productService,
            ICategoryService categoryService,
            IBrandService brandService,
            IProductUnitService productUnitService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _brandService = brandService;
            _productUnitService = productUnitService;
        }

        /// <summary>
        /// Create a new product
        /// </summary>
        /// <param name="model">Create product request model</param>
        /// <returns>Returns the created product details</returns>
        [ProducesResponseType(typeof(BaseResponse<ProductDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<ProductDto>), StatusCodes.Status500InternalServerError)]
        [HttpPost("create-product")]
        [OpenApiOperation("Create new product.", "")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest model, [FromServices] IValidator<CreateProductRequest> validator)
        {
            if (model == null)
            {
                return BadRequest("Product creation request can't be null.");
            }

            var response = await _productService.CreateProductAsync(model);
            if (response.Status)
            {
                return Ok(response);
            }
            return BadRequest(response.Message ?? "Product creation failed.");
        }

        /// <summary>
        /// Get all products
        /// </summary>
        /// <param></param>
        /// <returns>Returns a list of products</returns>
        [ProducesResponseType(typeof(BaseResponse<IList<ProductDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<IList<ProductDto>>), StatusCodes.Status500InternalServerError)]
        [HttpGet("get-all-products")]
        [OpenApiOperation("Get all products.", "")]
        public async Task<IActionResult> GetAllProducts()
        {
            var response = await _productService.GetAllProduct();
            if (response.Status)
            {
                return Ok(response);
            }
            return BadRequest(response.Message ?? "Failed to retrieve products.");
        }


        /// <summary>
        /// Get product by id
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns>Returns product details</returns>
        [ProducesResponseType(typeof(BaseResponse<ProductDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<ProductDto>), StatusCodes.Status500InternalServerError)]
        [HttpGet("get-product-by-id/{id}")]
        [OpenApiOperation("Get product by ID.", "")]
        public async Task<IActionResult> GetProductById([FromRoute] string id)
        {
            var response = await _productService.GetProductDetails(id);
            if (response != null)
            {
                return Ok(response);
            }
            return NotFound("Product not found.");
        }

        /// <summary>
        /// Delete product by id
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns>Returns a success or failure response</returns>
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
        [HttpDelete("delete-product/{id}")]
        [OpenApiOperation("Delete product by ID.", "")]
        public async Task<IActionResult> DeleteProduct([FromRoute] string id)
        {
            var response = await _productService.DeleteProduct(id);
            if (response.Status)
            {
                return Ok(response);
            }
            return BadRequest(response.Message ?? "Failed to delete product.");
        }


        /// <summary>
        /// Update an existing product
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <param name="model">Update product request model</param>
        /// <returns>Returns the updated product details</returns>
        [ProducesResponseType(typeof(BaseResponse<ProductDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<ProductDto>), StatusCodes.Status500InternalServerError)]
        [HttpPut("update-product/{id}")]
        [OpenApiOperation("Update product by ID.", "")]
        public async Task<IActionResult> UpdateProduct([FromRoute] string id,[FromForm] UpdateProductRequest model, [FromServices] IValidator<UpdateProductRequest> validator)
        {
            if (id == null)
            {
                return BadRequest("Product ID can't be null or empty.");
            }

            var response = await _productService.UpdateProduct(id, model);
            if (response.Status)
            {
                return Ok(response);
            }
            return BadRequest(response.Message ?? "Product update failed.");
        }

        ///// <summary>
        ///// Upload products from an Excel file
        ///// </summary>
        ///// <param name="excelFile">Excel file containing product data</param>
        ///// <returns>Returns a success or failure response</returns>
        //[ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
        //[HttpPost("upload-products-from-excel")]
        //[OpenApiOperation("Upload products from an Excel file.", "")]
        //public async Task<IActionResult> UploadProductsFromExcel([FromForm] IFormFile excelFile)
        //{
        //    if (excelFile == null || excelFile.Length == 0)
        //    {
        //        return BadRequest("Please select a valid Excel file.");
        //    }

        //    try
        //    {
        //        using var stream = excelFile.OpenReadStream();
        //        await _productService.UploadProductsFromExcelAsync(stream);
        //        return Ok(new { message = "Products uploaded successfully." });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
        //    }
        //}

        /// <summary>
        /// Download product template
        /// </summary>
        /// <returns>Returns the product template file</returns>
        [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
        [HttpGet("download-product-template")]
        [OpenApiOperation("Download product template.", "")]
        public async Task<IActionResult> DownloadProductTemplate()
        {
            try
            {
                var fileResult = await _productService.DownloadProductTemplateAsync();
                return fileResult;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while generating the product template: {ex.Message}");
            }
        }

    }

}
