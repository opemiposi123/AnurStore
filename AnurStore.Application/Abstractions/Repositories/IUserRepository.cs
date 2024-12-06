using AnurStore.Domain.Entities;

namespace AnurStore.Application.Abstractions.Repositories;

public interface IUserRepository
{
    Task<User> AddAsync(User user); 
    Task<User> GetByIdAsync(string id);
    Task<IList<User>> GetAllAsync(); // To fetch all users asynchronously
    Task<bool> UpdateAsync(User user); // To update an existing user
    Task<bool> DeleteAsync(string id); // To delete a user by ID asynchronously
    Task<bool> ExistsByUsernameAsync(string username); // To check if a user exists by username
    Task<User> GetByUsernameAsync(string username); // To fetch a user by username
}

