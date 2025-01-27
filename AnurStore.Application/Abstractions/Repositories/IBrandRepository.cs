using AnurStore.Domain.Entities;

namespace AnurStore.Application.Abstractions.Repositories
{
    public interface IBrandRepository
    {
        Task<Brand> CreateBrand(Brand brand);
        Task<IList<Brand>> GetAllBrands();
        Task<Brand> GetBrandById(string id);
        Task<bool> UpdateBrand(Brand Brand);
        Task<bool> Exist(string brandName); 
        List<Brand> GetAllBrand();
        Task<Brand?> GetBrandByNameAsync(string brandName);
    }
}