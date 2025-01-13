using AnurStore.Application.Abstractions.Repositories;
using AnurStore.Domain.Entities;
using AnurStore.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace AnurStore.Persistence.Repositories
{
    public class ProductSizeSizeRepository : IProductSizeRepository
    {
        private readonly ApplicationContext _context;

        public ProductSizeSizeRepository(ApplicationContext context) 
        {
            _context = context;
        }
        public async Task<ProductSize> CreateProductSize(ProductSize ProductSize)
        {
            var result = await _context.AddAsync(ProductSize);
            await _context.SaveChangesAsync();
            return ProductSize;
        }

        public async Task<IList<ProductSize>> GetAllProductSize()
        {
            var produtcs = await _context.ProductSizes
                  .ToListAsync();
            return produtcs;
        }
        public async Task<bool> UpdateProductSize(ProductSize ProductSize)
        {
            var result = _context.ProductSizes.Update(ProductSize);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
