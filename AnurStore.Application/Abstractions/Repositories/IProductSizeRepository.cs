using AnurStore.Domain.Entities;

namespace AnurStore.Application.Abstractions.Repositories
{
    public interface IProductSizeRepository
    {
        Task<ProductSize> CreateProductSize(ProductSize ProductSize);
        Task<IList<ProductSize>> GetAllProductSizes();
        Task<ProductSize> GetProductSizeById(string id);
        Task<bool> UpdateProductSize(ProductSize ProductSize);
        Task<bool> Exist(string ProductSizeName);
        List<ProductSize> GetAllProductSize();
    }
}
