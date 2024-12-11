using AnurStore.Application.Abstractions.Repositories;
using AnurStore.Domain.Entities;
using AnurStore.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace AnurStore.Persistence.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly ApplicationContext _context;

        public SupplierRepository(ApplicationContext context)
        {
            _context = context;

        }

        public async Task<Supplier> CreateSupplier(Supplier supplier)
        {
            var result = await _context.AddAsync(supplier);
            await _context.SaveChangesAsync();
            return supplier;
        }

        public async Task<bool> Exist(string supplierName)
        {
            var supplier = await _context.Categories.AnyAsync(r => r.Name == supplierName);
            return supplier;
        }

        public List<Supplier> GetAllSupplier()
        { 
            return _context.Suppliers.ToList();
        }

        public async Task<IList<Supplier>> GetAllSuppliers()
        {
            var suppliers = await _context.Suppliers
                 .ToListAsync();
            return suppliers;
        }

        public async Task<Supplier> GetSupplierById(string id)
        {
            return await _context.Suppliers.FindAsync(id);
        }

        public async Task<bool> UpdateSupplier(Supplier supplier)
        {
            var result = _context.Suppliers.Update(supplier);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
