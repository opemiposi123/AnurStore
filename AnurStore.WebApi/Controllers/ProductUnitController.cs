using AnurStore.Application.Abstractions.Services;
using AnurStore.Application.DTOs;
using AnurStore.Application.RequestModel;
using AnurStore.Application.Wrapper;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace AnurStore.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductUnitController : ControllerBase
    {
        private readonly IProductUnitService _productUnitService;

        public ProductUnitController(IProductUnitService productUnitService)
        {
            _productUnitService = productUnitService;
        }

        /// <summary>
        /// Create a new product unit.
        /// </summary>
        /// <param name="model">Product unit creation request model</param>
        /// <returns>Returns the created product unit details</returns>
        [HttpPost("create")]
        [OpenApiOperation("Create a new product unit.", "")]
        [ProducesResponseType(typeof(BaseResponse<ProductUnitDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProductUnit([FromBody] CreateProductUnitRequest model)
        {
            if (model == null)
                return BadRequest("Product unit creation request can't be null.");

            var response = await _productUnitService.CreateProductUnit(model);
            if (response.Status)
                return Ok(response);

            return BadRequest(response.Message ?? "Product unit creation failed.");
        }

        /// <summary>
        /// Get all product units.
        /// </summary>
        /// <returns>Returns a list of all product units</returns>
        [HttpGet("get-all")]
        [OpenApiOperation("Get all product units.", "")]
        [ProducesResponseType(typeof(BaseResponse<IList<ProductUnitDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllProductUnits()
        {
            var response = await _productUnitService.GetAllProductUnit();
            if (response.Status)
                return Ok(response);

            return BadRequest(response.Message ?? "Failed to retrieve product units.");
        }

        /// <summary>
        /// Get product unit by ID.
        /// </summary>
        /// <param name="id">Product unit ID</param>
        /// <returns>Returns product unit details</returns>
        [HttpGet("get/{id}")]
        [OpenApiOperation("Get product unit by ID.", "")]
        [ProducesResponseType(typeof(BaseResponse<ProductUnitDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductUnitById([FromRoute] string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Product unit ID can't be null or empty.");

            var response = await _productUnitService.GetProductUnit(id);
            if (response != null)
                return Ok(response);

            return NotFound("Product Unit not found.");
        }

        /// <summary>
        /// Update an existing product unit.
        /// </summary>
        /// <param name="id">Product unit ID</param>
        /// <param name="model">Update product unit request model</param>
        /// <returns>Returns the updated product unit details</returns>
        [HttpPut("update/{id}")]
        [OpenApiOperation("Update a product unit.", "")]
        [ProducesResponseType(typeof(BaseResponse<ProductUnitDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateProductUnit([FromRoute] string id, [FromBody] UpdateProductUnitRequest model)
        {
            if (id == null)
                return BadRequest("Product unit ID can't be null or empty.");

            if (model == null)
                return BadRequest("Update request can't be null.");

            var response = await _productUnitService.UpdateProductUnit(id, model);
            if (response.Status)
                return Ok(response);

            return BadRequest(response.Message ?? "Product unit update failed.");
        }

        /// <summary>
        /// Delete a product unit by ID.
        /// </summary>
        /// <param name="id">Product unit ID</param>
        /// <returns>Returns success or failure response</returns>
        [HttpDelete("delete/{id}")]
        [OpenApiOperation("Delete a product unit by ID.", "")]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteProductUnit([FromRoute] string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Product unit ID can't be null or empty.");

            var response = await _productUnitService.DeleteProductUnit(id);
            if (response.Status)
                return Ok(response);

            return BadRequest(response.Message ?? "Failed to delete product unit.");
        }
    }
}
