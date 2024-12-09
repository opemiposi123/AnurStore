using AnurStore.Application.Abstractions.Repositories;
using AnurStore.Domain.Entities;
using AnurStore.Domain.Enums;
using AnurStore.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace AnurStore.Persistence.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly ApplicationContext _context;

        public BrandRepository(ApplicationContext context)
        {
            _context = context;

        }
        public async Task<Brand> CreateBrand(Brand brand)
        {
            var result = await _context.AddAsync(brand);
            await _context.SaveChangesAsync();
            return brand;
        }

        public async Task<bool> Exist(string brandName)
        {
            var brand = await _context.Brands.AnyAsync(r => r.Name == brandName);
            return brand;
        }

        public List<Brand> GetAllBrand()
        {
            return _context.Brands.ToList();
        }

        public async Task<IList<Brand>> GetAllBrands()
        {
            var brands = await _context.Brands
                .ToListAsync();
            return brands;
        }

        public async Task<Brand> GetBrandById(string id)
        {
            return await _context.Brands.FindAsync(id);
        }

        public async Task<bool> UpdateBrand(Brand brand)
        {
            var result = _context.Brands.Update(brand);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
