using AnurStore.Application.DTOs;
using AnurStore.Application.RequestModel;
using AnurStore.Application.Wrapper;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AnurStore.Application.Abstractions.Services
{
    public interface IProductUnitService
    {
        Task<BaseResponse<string>> CreateProductUnit(CreateProductUnitRequest request);
        Task<BaseResponse<bool>> UpdateProductUnit(string productUnitId, UpdateProductUnitRequest request);
        Task<BaseResponse<ProductUnitDto>> GetProductUnit(string productUnitId);
        Task<BaseResponse<IEnumerable<ProductUnitDto>>> GetAllProductUnit();
        Task<BaseResponse<bool>> DeleteProductUnit(string productUnitId);
        Task<IEnumerable<SelectListItem>> GetProductUnitList();
    }
}
 