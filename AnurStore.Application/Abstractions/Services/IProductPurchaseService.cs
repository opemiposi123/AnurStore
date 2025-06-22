using AnurStore.Application.DTOs;
using AnurStore.Application.Pagination;
using AnurStore.Application.RequestModel;
using AnurStore.Application.Wrapper;

namespace AnurStore.Application.Abstractions.Services
{
    public interface IProductPurchaseService
    {
        Task<BaseResponse<string>> PurchaseProductsAsync(CreateProductPurchaseRequest request, string userName);
        Task<BaseResponse<ProductPurchaseDto>> GetPurchaseDetailsAsync(string purchaseId); 
        Task<BaseResponse<IEnumerable<ProductPurchaseDto>>> GetAllPurchasesAsync();
        Task<BaseResponse<IEnumerable<ProductPurchaseDto>>> GetPurchasesBySupplierAsync(string supplierId);
        Task<BaseResponse<bool>> DeletePurchaseAsync(string purchaseId);
        Task<BaseResponse<IEnumerable<ProductPurchaseDto>>> GetPurchasesByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<BaseResponse<IEnumerable<ProductPurchaseDto>>> GetPurchasesByProductAsync(string productId);
        Task<BaseResponse<byte[]>> ExportPurchasesToExcelAsync(PurchaseExportRequest request);
        Task<BaseResponse<PaginatedResult<ProductPurchaseDto>>> GetPurchasesPagedAsync(PurchaseFilterRequest filter);
    }
}
