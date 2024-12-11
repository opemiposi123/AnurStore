using AnurStore.Domain.Entities;

namespace AnurStore.Application.Abstractions.Repositories;

public interface IUserRepository
{
    Task<User> GetByIdAsync(string id);
    Task<IList<User>> GetAllAsync(); 
    Task<bool> UpdateAsync(User user); 
    Task<bool> DeleteAsync(string id); 
    Task<bool> ExistsByUsernameAsync(string username); 
    Task<User> GetByUsernameAsync(string username); 
}

