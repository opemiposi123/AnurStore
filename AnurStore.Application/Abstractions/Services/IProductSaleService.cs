using AnurStore.Application.DTOs;
using AnurStore.Application.RequestModel;
using AnurStore.Application.Wrapper;

namespace AnurStore.Application.Abstractions.Services
{
    public interface IProductSaleService
    {
        Task<BaseResponse<byte[]>> AddProductSale(CreateProductSaleRequest request);
        Task<BaseResponse<ProductSaleDto>> GetProductSaleById(string productSaleId);
        Task<BaseResponse<List<ProductSaleDto>>> GetAllProductSalesAsync();
        Task<BaseResponse<PagedResponse<ProductSaleDto>>> GetFilteredProductSalesPagedAsync(ProductSaleFilterRequest filter);
        Task<BaseResponse<bool>> CancelProductSaleAsync(string saleId);
        Task<BaseResponse<bool>> UpdateProductSaleAsync(UpdateProductSaleRequest request);
    }
}
