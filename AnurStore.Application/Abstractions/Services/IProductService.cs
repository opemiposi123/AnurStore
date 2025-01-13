using AnurStore.Application.DTOs;
using AnurStore.Application.RequestModel;
using AnurStore.Application.Wrapper;
using Microsoft.AspNetCore.Http;

namespace AnurStore.Application.Abstractions.Services
{
    public interface IProductService
    {
        Task<BaseResponse<string>> CreateProductAsync(CreateProductRequest request);
        Task<BaseResponse<bool>> UpdateProduct(string productId, UpdateProductRequest request); 
        Task<BaseResponse<ProductDto>> GetProduct(string productId);
        Task<BaseResponse<IEnumerable<ProductDto>>> GetAllProduct(); 
        Task<BaseResponse<bool>> DeleteProduct(string productId);
        Task<string> SaveFileAsync(IFormFile file);
    }
}
