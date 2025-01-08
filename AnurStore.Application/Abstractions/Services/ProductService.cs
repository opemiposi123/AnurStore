using AnurStore.Application.DTOs;
using AnurStore.Application.RequestModel;
using AnurStore.Application.Wrapper;

namespace AnurStore.Application.Abstractions.Services
{
    public interface ProductService
    {
        Task<BaseResponse<string>> CreateProduct(CreateProductRequest request);
        Task<BaseResponse<bool>> UpdateProduct(string productId, UpdateProductRequest request); 
        Task<BaseResponse<ProductDto>> GetProduct(string productId);
        Task<BaseResponse<IEnumerable<ProductDto>>> GetAllProduct();
        Task<BaseResponse<bool>> DeleteProduct(string productId);
    }
}
