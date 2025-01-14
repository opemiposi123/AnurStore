using AnurStore.Application.DTOs;
using AnurStore.Application.RequestModel;
using AnurStore.Application.Wrapper;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AnurStore.Application.Abstractions.Services
{
    public interface ICategoryService
    {
        Task<BaseResponse<string>> CreateCategory(CreateCategoryRequest request);
        Task<BaseResponse<bool>> UpdateCategory(string CategoryId, UpdateCategoryRequest request);
        Task<BaseResponse<CategoryDto>> GetCategory(string CategoryId);
        Task<BaseResponse<IEnumerable<CategoryDto>>> GetAllCategory();
        Task<BaseResponse<bool>> DeleteCategory(string categoryId);
        Task<IEnumerable<SelectListItem>> GetCategorySelectList();
    }
}
 