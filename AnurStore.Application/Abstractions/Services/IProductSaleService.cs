using AnurStore.Application.DTOs;
using AnurStore.Application.RequestModel;
using AnurStore.Application.Wrapper;

namespace AnurStore.Application.Abstractions.Services
{
    public interface IProductSaleService
    {
        Task<BaseResponse<byte[]>> AddProductSale(CreateProductSaleRequest request);
        Task<BaseResponse<ProductSaleDto>> GetProductSaleById(string productSaleId);
        Task<PagedResponse<List<ProductSaleDto>>> GetAllProductSalesPagedAsync(int pageNumber, int pageSize);
        Task<PagedResponse<List<ProductSaleDto>>> GetFilteredProductSalesPagedAsync(ProductSaleFilterRequest filter);
        Task<BaseResponse<bool>> CancelProductSaleAsync(string saleId);
        Task<BaseResponse<bool>> UpdateProductSaleAsync(string productSaleId, UpdateProductSaleRequest request);
        Task<BaseResponse<CreateProductSaleRequest>> PrepareSaleRequestAsync(CreateProductSaleViewModel viewModel);
    }
}
