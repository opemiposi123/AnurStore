using AnurStore.Application.DTOs;
using AnurStore.Application.RequestModel;
using AnurStore.Application.Wrapper;

namespace AnurStore.Application.Abstractions.Services
{
    public interface IBrandService
    {
        Task<BaseResponse<string>> CreateBrand(CreateBrandRequest request);
        Task<BaseResponse<bool>> UpdateBrand(string BrandId, UpdateBrandRequest request);
        Task<BaseResponse<BrandDto>> GetBrand(string BrandId); 
        Task<BaseResponse<IEnumerable<BrandDto>>> GetAllBrand();
        Task<BaseResponse<bool>> DeleteBrand(string BrandId);
    }
}