using AnurStore.Domain.Entities;

namespace AnurStore.Application.Abstractions.Repositories;

public interface ICategoryRepository
{
    Task<Category> CreateCategory(Category Category);
    Task<IList<Category>> GetAllCategories();
    Task<Category> GetCategoryById(string id);
    Task<bool> UpdateCategory(Category Category);
    Task<bool> Exist(string CategoryName);
    List<Category> GetAllCategory();
    Task<Category?> GetCategoryByNameAsync(string categoryName);
}