using AnurStore.Application.Abstractions.Repositories;
using AnurStore.Domain.Entities;
using AnurStore.Persistence.Context;
using Microsoft.EntityFrameworkCore;

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

        public async Task<bool> Exist(string productName)
        {
            var product = await _context.Products.AnyAsync(r => r.Name == productName);
            return product;
        }

        public async Task<IList<Product>> GetAllProduct() 
        {
            var produtcs = await _context.Products
                  .Include(r  => r.ProductSize)
                     .ThenInclude(r => r.ProductUnit)   
                  .Include(r => r.Category)
                  .Include(r => r.Brand)
                  .ToListAsync();
            return produtcs;
        }

        public async Task<Product> GetProductById(string id)
        {
            return await _context.Products.FindAsync(id);
        }

        public List<Product> SelectProduct()
        {
            return _context.Products.ToList();
        }

        public async  Task<bool> UpdateProduct(Product product)
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

        public async Task<ProductSize?> GetProductSizeByProductIdAsync(string productId)
        {
            if (string.IsNullOrWhiteSpace(productId))
                throw new ArgumentException("ProductId cannot be null or empty.", nameof(productId));

            return await _context.ProductSizes
                .AsNoTracking() // Improves performance if no modifications are needed
                .FirstOrDefaultAsync(ps => ps.ProductId == productId);
        }
    }
}
