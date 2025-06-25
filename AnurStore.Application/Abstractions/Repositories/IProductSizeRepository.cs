using AnurStore.Domain.Entities;

namespace AnurStore.Application.Abstractions.Repositories
{
    public interface IProductSizeRepository
    {
        Task<ProductSize> CreateProductSize(ProductSize ProductSize);
        Task<IList<ProductSize>> GetAllProductSize();
        Task UpdateProductSize(ProductSize productSize);
    }
}
