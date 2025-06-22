using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AnurStore.Application.Abstractions.Services;
using AnurStore.Application.DTOs;
using AnurStore.Application.RequestModel;
using FluentValidation;

namespace AnurStore.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService) 
        {
            _brandService = brandService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BrandDto>>> GetAllBrands()
        {
            var response = await _brandService.GetAllBrand();
            if (response.Status)
            {
                return Ok(response.Data);
            }
            return Ok(Enumerable.Empty<BrandDto>());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BrandDto>> GetBrandDetail(string id)
        {
            var brand = await _brandService.GetBrand(id);
            if (brand == null)
            {
                return NotFound();
            }
            return Ok(brand);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateBrand(
            [FromBody] CreateBrandRequest model,
            [FromServices] IValidator<CreateBrandRequest> validator)
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var response = await _brandService.CreateBrand(model);
            if (response.Status)
            {
                return CreatedAtAction(nameof(GetBrandDetail), new { id = response }, response.Data);
            }

            return BadRequest(response.Message);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("{id}")]
        public async Task<IActionResult> DeleteBrand(string id)
        {
            var response = await _brandService.DeleteBrand(id);
            if (response.Status)
            {
                return Ok("Brand Deleted Successfully");
            }

            return NotFound("Brand not found or could not be deleted");
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateBrand(
            string id,
            [FromBody] UpdateBrandRequest model,
            [FromServices] IValidator<UpdateBrandRequest> validator)
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var response = await _brandService.UpdateBrand(id, model);
            if (response.Status)
            {
                return Ok("Brand Updated Successfully");
            }

            return NotFound("Brand not found or could not be updated");
        }
    }

}
