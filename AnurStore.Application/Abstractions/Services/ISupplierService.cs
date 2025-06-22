using AnurStore.Application.DTOs;
using AnurStore.Application.RequestModel;
using AnurStore.Application.Wrapper;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AnurStore.Application.Abstractions.Services
{
    public interface ISupplierService 
    {
        Task<BaseResponse<string>> CreateSupplier(CreateSupplierRequest request);
        Task<BaseResponse<bool>> UpdateSupplier(string supplierId, UpdateSupplierRequest request);
        Task<BaseResponse<SupplierDto>> GetSupplier(string supplierId); 
        Task<BaseResponse<IEnumerable<SupplierDto>>> GetAllSupplier();
        Task<BaseResponse<bool>> DeleteSupplier(string supplierId);
        Task<IEnumerable<SelectListItem>> GetSupplierSelectList(); 
    }
}
