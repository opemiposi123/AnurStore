using AnurStore.Application.Abstractions.Repositories;
using AnurStore.Application.Abstractions.Services;
using AnurStore.Application.DTOs;
using AnurStore.Application.RequestModel;
using AnurStore.Application.Wrapper;
using AnurStore.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace AnurStore.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<CategoryService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CategoryService(ICategoryRepository categoryRepository, ILogger<CategoryService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse<string>> CreateCategory(CreateCategoryRequest request)
        {
            var userName = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "System";
            _logger.LogInformation("Starting CreateCategory method.");
            try
            {
                if (request == null)
                {
                    _logger.LogWarning("CreateCategory request is null.");
                    return new BaseResponse<string>
                    {
                        Message = "Request cannot be null",
                        Status = false,
                    };
                }
                _logger.LogInformation("Checking if category with name {CategoryName} exists.", request.Name);
                var reportExists = await _categoryRepository.Exist(request.Name);

                if (reportExists)
                {
                    _logger.LogWarning("Category with name {CategoryName} already exists.", request.Name);
                    return new BaseResponse<string>
                    {
                        Status = false,
                        Message = "Category name already exists"
                    };
                }

                var category = new Category
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = request.Name,
                    Description = request.Description,
                    CreatedBy = userName,
                    CreatedOn = DateTime.Now
                };

                _logger.LogInformation("Creating new category {CategoryName}.", request.Name);
                var createdCategory = await _categoryRepository.CreateCategory(category);

                _logger.LogInformation("Report type created successfully with Id {CategoryId}.", category.Id);
                return new BaseResponse<string>
                {
                    Message = "Category created successfully",
                    Status = true,
                    Data = category.Id,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating category.");
                return new BaseResponse<string>
                {
                    Message = $"Failed to create category: {ex.Message}",
                    Status = false,
                };
            }
        } 

        public async Task<BaseResponse<bool>> DeleteCategory(string categoryId)
        {
            try
            {
                var category = await _categoryRepository.GetCategoryById(categoryId);
                if (category == null)
                {
                    _logger.LogWarning($"Category with Id {categoryId} not found.");
                    return new BaseResponse<bool>
                    {
                        Message = $"Category not found",
                        Status = false,
                    };
                }

                category.IsDeleted = true;

                var result = await _categoryRepository.UpdateCategory(category);

                if (result)
                {
                    _logger.LogInformation($"Category with Id {categoryId} deleted successfully.");
                    return new BaseResponse<bool>
                    {
                        Message = $"Category deleted successfuly",
                        Status = false,
                    };
                }
                else
                {
                    _logger.LogError($"Failed to delete category with Id {categoryId}.");
                    return new BaseResponse<bool>
                    {
                        Message = $"Fail to delete category",
                        Status = false,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting Category with Id {categoryId}.", categoryId);
                return new BaseResponse<bool>
                {
                    Message = $"An error occured {ex.Message}",
                    Status = false,
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<CategoryDto>>> GetAllCategory() 
        {
            _logger.LogInformation("Starting GetAllCategory method.");
            try
            {
                var report = await _categoryRepository.GetAllCategories();
                var categoryDtos = report.Where(x => !x.IsDeleted).Select(r => new CategoryDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description
                }).ToList();
                 
                _logger.LogInformation("Successfully retrieved categories."); 
                return new BaseResponse<IEnumerable<CategoryDto>>
                {
                    Message = "Record Found Successfully",
                    Status = true,
                    Data = categoryDtos,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving categories.");
                return new BaseResponse<IEnumerable<CategoryDto>>
                {
                    Status = false,
                    Message = "Failed",
                };
            }
        }

        public async Task<BaseResponse<CategoryDto>> GetCategory(string categoryId)
        {
            _logger.LogInformation("Starting GetCategory method for Id {CategoryId}.", categoryId);
            try
            {
                var category = await _categoryRepository.GetCategoryById(categoryId);
                if (category == null)
                {
                    _logger.LogWarning("Category with Id {CategoryId} not found.", categoryId);
                    return new BaseResponse<CategoryDto>
                    {
                        Message = "Category not found",
                        Status = false
                    };
                }

                var categoryDto = new CategoryDto
                {
                    Id = categoryId,
                    Name = category.Name,
                    Description = category.Description,
                    CreatedBy = category.CreatedBy,
                    LastModifiedBy = category.LastModifiedBy,
                    CreatedOn = category.CreatedOn,
                    LastModifiedOn = category.LastModifiedOn
                };

                _logger.LogInformation("Successfully retrieved category with Id {CategoryId}.", categoryId);
                return new BaseResponse<CategoryDto>
                {
                    Data = categoryDto,
                    Status = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving category with Id {CategoryId}.", categoryId);
                return new BaseResponse<CategoryDto>
                {
                    Message = $"Failed to retrieve category: {ex.Message}",
                    Status = false
                };
            }
        } 

        public async Task<BaseResponse<bool>> UpdateCategory(string categoryId, UpdateCategoryRequest request)
        {
            var userName = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "System";
            _logger.LogInformation("Starting UpdateCategory method for Id {CategoryId}.", categoryId);
            try
            {
                var existingCategory = await _categoryRepository.GetCategoryById(categoryId); 
                if (existingCategory == null)
                {
                    _logger.LogWarning($"Category with Id {categoryId} not found.");
                    return new BaseResponse<bool>
                    {
                        Message = "Category not found",
                        Status = false
                    };
                }

                existingCategory.Description = request.Description;
                existingCategory.Name = request.Name;

                await _categoryRepository.UpdateCategory(existingCategory);

                var Category = new Category
                {
                    LastModifiedBy = userName,
                    LastModifiedOn = existingCategory.LastModifiedOn,
                    Description = existingCategory.Description,
                };
                _logger.LogInformation("Successfully updated category with Id {CategoryId}.", categoryId);
                return new BaseResponse<bool>
                {
                    Data = true,
                    Message = "Category updated successfully",
                    Status = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating category with Id {CategoryId}.", categoryId);
                return new BaseResponse<bool>
                {
                    Message = $"Failed to update category: {ex.Message}",
                    Status = false
                };
            }
        }

        public async Task<IEnumerable<SelectListItem>> GetCategorySelectList()
        {
            var categoryResponse = await GetAllCategory();
            if (categoryResponse.Status && categoryResponse.Data != null)
            {
                var categoryList = categoryResponse.Data.Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name
                });
                return categoryList;
            }
            return Enumerable.Empty<SelectListItem>();
        }
    }
} 