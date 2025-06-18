using AnurStore.Application.Abstractions.Services;
using AnurStore.Application.DTOs;
using AnurStore.Application.RequestModel;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace AnurStore.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllCategories()
        {
            var response = await _categoryService.GetAllCategory();
            if (response.Status)
            {
                return Ok(response.Data);
            }

            return Ok(Enumerable.Empty<CategoryDto>());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategoryDetail(string id)
        {
            var response = await _categoryService.GetCategory(id);
            if (response == null || response.Data == null)
            {
                return NotFound("Category not found.");
            }

            return Ok(response.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(
            [FromBody] CreateCategoryRequest model,
            [FromServices] IValidator<CreateCategoryRequest> validator)
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var response = await _categoryService.CreateCategory(model);
            if (response.Status)
            {
                return CreatedAtAction(nameof(GetCategoryDetail), new { id = response }, response.Data);
            }

            return BadRequest(response.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            var response = await _categoryService.DeleteCategory(id);
            if (response.Status)
            {
                return Ok("Category deleted successfully.");
            }

            return NotFound("Category not found or could not be deleted.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(
            string id,
            [FromBody] UpdateCategoryRequest model,
            [FromServices] IValidator<UpdateCategoryRequest> validator)
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var response = await _categoryService.UpdateCategory(id, model);
            if (response.Status)
            {
                return Ok("Category updated successfully.");
            }

            return NotFound("Category not found or could not be updated.");
        }
    }
}
