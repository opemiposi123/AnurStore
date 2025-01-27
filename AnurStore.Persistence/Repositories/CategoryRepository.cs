using AnurStore.Application.Abstractions.Repositories;
using AnurStore.Domain.Entities;
using AnurStore.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace AnurStore.Persistence.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationContext _context;

        public CategoryRepository(ApplicationContext context)
        {
            _context = context;

        }

        public async Task<Category> CreateCategory(Category category)
        {
            var result = await _context.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<bool> Exist(string categoryName)
        {
            var category = await _context.Categories.AnyAsync(r => r.Name == categoryName);
            return category;
        }

        public List<Category> GetAllCategory()
        {
            return _context.Categories.ToList();
        }

        public async Task<IList<Category>> GetAllCategories()
        {
            var categories = await _context.Categories
                 .ToListAsync();
            return categories;
        }

        public async Task<Category> GetCategoryById(string id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<bool> UpdateCategory(Category category)
        {
            var result = _context.Categories.Update(category);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Category> GetCategoryByNameAsync(string categoryName)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.Name.ToLower() == categoryName.ToLower());
        }


    }
}
