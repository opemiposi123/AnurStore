using AnurStore.Application.Abstractions.Services;
using AnurStore.Application.DTOs;
using AnurStore.Application.RequestModel;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using FluentValidation;
using AnurStore.Application.Wrapper;

namespace AnurStore.Api.Controllers
{
    [Route("api/suppliers")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        /// <summary>
        /// Create a new supplier
        /// </summary>
        /// <param name="model">CreateSupplierRequest model</param>
        /// <returns>Operation status</returns>
        [HttpPost("create-supplier")]
        [ProducesResponseType(typeof(BaseResponse<SupplierDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<SupplierDto>), StatusCodes.Status400BadRequest)]
        [OpenApiOperation("Create a new supplier.", "")]
        public async Task<IActionResult> CreateSupplier([FromBody] CreateSupplierRequest model, [FromServices] IValidator<CreateSupplierRequest> validator)
        {
            if (model == null)
            {
                return BadRequest("Supplier creation request can't be null.");
            }
            var response = await _supplierService.CreateSupplier(model);
            if (response.Status)
            {
                return Ok(response);
            }
            return BadRequest(response.Message ?? "Supplier creation failed.");
        }


        /// <summary>
        /// Get all suppliers
        /// </summary>
        /// <returns>A list of suppliers</returns>
        [HttpGet("get-all-suppliers")]
        [ProducesResponseType(typeof(BaseResponse<IList<SupplierDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<IList<SupplierDto>>), StatusCodes.Status500InternalServerError)]
        [OpenApiOperation("Get all suppliers.", "")]
        public async Task<IActionResult> GetAllSuppliers()
        {
            var response = await _supplierService.GetAllSupplier();
            if (response.Status)
            {
                return Ok(response);
            }
            return BadRequest(response.Message ?? "Failed to retrieve suppliers.");
        }

        /// <summary>
        /// Get supplier by ID
        /// </summary>
        /// <param name="id">Supplier ID</param>
        /// <returns>Supplier details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BaseResponse<SupplierDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<SupplierDto>), StatusCodes.Status404NotFound)]
        [OpenApiOperation("Get supplier by ID.", "")]
        public async Task<IActionResult> GetSupplierById(string id)
        {
            var response = await _supplierService.GetSupplier(id); 
            if (response != null)
            {
                return Ok(response);
            }
            return NotFound("supplier not found.");
        }



        /// <summary>
        /// Update supplier
        /// </summary>
        /// <param name="id">Supplier ID</param>
        /// <param name="model">UpdateSupplierRequest model</param>
        /// <returns>Operation status</returns>
        [HttpPut("update-supplier/{id}")]
        [ProducesResponseType(typeof(BaseResponse<SupplierDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<SupplierDto>), StatusCodes.Status400BadRequest)]
        [OpenApiOperation("Update an existing supplier.", "")]
        public async Task<IActionResult> UpdateSupplier(string id, [FromBody] UpdateSupplierRequest model, [FromServices] IValidator<UpdateSupplierRequest> validator)
        {
            if (id == null)
            {
                return BadRequest("Product ID can't be null or empty.");
            }

            var response = await _supplierService.UpdateSupplier(id, model);
            if (response.Status)
            {
                return Ok(response);
            }
            return BadRequest(response.Message ?? "supplier update failed.");
        }


        /// <summary>
        /// Delete a supplier
        /// </summary>
        /// <param name="id">Supplier ID</param>
        /// <returns>Operation status</returns>
        [HttpDelete("delete-supplier/{id}")]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status404NotFound)]
        [OpenApiOperation("Delete a supplier.", "")]
        public async Task<IActionResult> DeleteSupplier(string id)
        {
            var response = await _supplierService.DeleteSupplier(id);
            if (response.Status)
            {
                return Ok(response);
            }
            return BadRequest(response.Message ?? "Failed to delete supplier.");
        }
         
    }
}
