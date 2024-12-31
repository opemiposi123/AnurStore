using AnurStore.Application.Abstractions.Repositories;
using AnurStore.Domain.Entities;
using AnurStore.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace AnurStore.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext _context;

        public UserRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<User> GetByIdAsync(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<IList<User>> GetAllAsync()
        {
            var users = await _context.Users
              .ToListAsync();
            return users;
        }

        public async Task<bool> UpdateAsync(User user)
        {
            _context.Users.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var user = await GetByIdAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExistsByUsernameAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.UserName == username);
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync();
        }
    }
}

