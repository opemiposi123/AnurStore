using AnurStore.Application.Abstractions.Repositories;
using AnurStore.Domain.Entities;
using AnurStore.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace AnurStore.Persistence.Repositories
{
    public class ProductSizeRepository : IProductSizeRepository
    {
        private readonly ApplicationContext _context;

        public ProductSizeRepository(ApplicationContext context)
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
        public async Task UpdateProductSize(ProductSize productSize)
        {
            var local = _context.ProductSizes.Local.FirstOrDefault(x => x.Id == productSize.Id);
            if (local != null)
            {
                _context.Entry(local).State = EntityState.Detached; // Detach any tracked version
            }

            _context.ProductSizes.Attach(productSize); // Now attach the updated one
            _context.Entry(productSize).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

    }
}
