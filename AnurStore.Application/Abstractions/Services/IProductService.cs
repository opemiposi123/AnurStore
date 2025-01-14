﻿using AnurStore.Application.DTOs;
using AnurStore.Application.RequestModel;
using AnurStore.Application.Wrapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AnurStore.Application.Abstractions.Services
{
    public interface IProductService
    {
        Task<BaseResponse<string>> CreateProductAsync(CreateProductRequest request);
        Task<BaseResponse<bool>> UpdateProduct(string productId, UpdateProductRequest request); 
        Task<BaseResponse<ProductDto>> GetProductDetails(string productId);
        Task<BaseResponse<IEnumerable<ProductDto>>> GetAllProduct(); 
        Task<BaseResponse<bool>> DeleteProduct(string productId);
        Task<string> SaveFileAsync(IFormFile file);
        Task<IEnumerable<SelectListItem>> GetProductSelectList();
    }
}
