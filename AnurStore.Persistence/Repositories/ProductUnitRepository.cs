using AnurStore.Application.Abstractions.Repositories;
using AnurStore.Domain.Entities;
using AnurStore.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace AnurStore.Persistence.Repositories
{
    public class ProductUnitRepository : IProductUnitRepository
    {
        private readonly ApplicationContext _context;

        public ProductUnitRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<ProductUnit> CreateProductUnit(ProductUnit productUnit)
        {
            var result = await _context.AddAsync(productUnit);
            await _context.SaveChangesAsync();
            return productUnit;
        }

        public async Task<bool> Exist(string productUnitName)
        {
            var productUnit = await _context.ProductUnits.AnyAsync(r => r.Name == productUnitName);
            return productUnit;
        }

        public List<ProductUnit> SelectProductUnit()
        {
            return _context.ProductUnits.ToList();
        } 

        public async Task<IList<ProductUnit>> GetAllProductUnit()
        {
            var units = await _context.ProductUnits
                 .ToListAsync();
            return units;
        }

        public async Task<ProductUnit> GetProductUnitById(string id)
        {
            return await _context.ProductUnits.FindAsync(id);
        }

        public async Task<bool> UpdateProductUnit(ProductUnit unit)
        {
            var result = _context.ProductUnits.Update(unit);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<ProductUnit?> GetProductUnitByNameAsync(string unitName)
        {
            if (string.IsNullOrEmpty(unitName))
                throw new ArgumentException("Product unit name cannot be null or empty.", nameof(unitName));

            return await _context.ProductUnits
                .FirstOrDefaultAsync(u => u.Name.Equals(unitName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
