using AnurStore.Application.Abstractions.Repositories;
using AnurStore.Application.DTOs;
using AnurStore.Domain.Entities;
using AnurStore.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AnurStore.Persistence.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationContext _context;

        public ProductRepository(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<Product> CreateProduct(Product product)
        {
            var result = await _context.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<IEnumerable<ProductDto>> SearchProductsByNameAsync(string query)
        {
            var products = await _context.Products
               .Where(p => !p.IsDeleted && p.Name.ToLower().Contains(query.ToLower()))
                .Include(p => p.Category)
                .Include(p => p.ProductSize)
                 .ThenInclude(p => p.ProductUnit) 
                .ToListAsync();

            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                CategoryName = p.Category?.Name ?? "N/A",
                SizeWithUnit = $"{p.ProductSize.Size}{p.ProductSize.ProductUnit.Name}"
            });
        }


        public async Task<bool> Exist(string productName)
        {
            var product = await _context.Products.AnyAsync(r => r.Name == productName);
            return product;
        }

        public async Task<IList<Product>> GetAllProduct()
        {
            var produtcs = await _context.Products
                  .Include(r => r.ProductSize)
                     .ThenInclude(r => r.ProductUnit)
                  .Include(r => r.Category)
                  .Include(r => r.Brand)
                  .ToListAsync();
            return produtcs;
        }

        public async Task<Product?> GetProductById(string productId)
        {
            return await _context.Products
                .Include(p => p.Inventory) 
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.ProductSize)
                .ThenInclude(p => p.ProductUnit)
                .FirstOrDefaultAsync(p => p.Id == productId);
        }


        public List<Product> SelectProduct()
        {
            return _context.Products.ToList();
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var result = _context.Products.Update(product);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<string> CreateProductWithSizeAsync(Product product, ProductSize productSize)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                if (productSize != null)
                {
                    productSize.ProductId = product.Id;
                    _context.ProductSizes.Add(productSize);
                    await _context.SaveChangesAsync();
                }

                // Commit the transaction
                await transaction.CommitAsync();

                return product.Id;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<ProductSize?> GetProductSizeByProductIdAsync(string productId, bool noTracking = false)
        {
            var query = _context.ProductSizes.Where(p => p.ProductId == productId);
            return noTracking ? await query.AsNoTracking().FirstOrDefaultAsync()
                              : await query.FirstOrDefaultAsync();
        }

    }
}
