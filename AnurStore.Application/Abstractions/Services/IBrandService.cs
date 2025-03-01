﻿using AnurStore.Application.DTOs;
using AnurStore.Application.RequestModel;
using AnurStore.Application.Wrapper;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AnurStore.Application.Abstractions.Services
{
    public interface IBrandService
    {
        Task<BaseResponse<string>> CreateBrand(CreateBrandRequest request);
        Task<BaseResponse<bool>> UpdateBrand(string brandId, UpdateBrandRequest request);
        Task<BaseResponse<BrandDto>> GetBrand(string brandId); 
        Task<BaseResponse<IEnumerable<BrandDto>>> GetAllBrand();
        Task<BaseResponse<bool>> DeleteBrand(string brandId);
        Task<IEnumerable<SelectListItem>> GetBrandSelectList();
    }
}