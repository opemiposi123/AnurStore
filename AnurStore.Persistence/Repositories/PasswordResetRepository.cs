using AnurStore.Application.Abstractions.Repositories;
using AnurStore.Domain.Entities;
using AnurStore.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace AnurStore.Persistence.Repositories
{
    public class PasswordResetRepository : IPasswordResetRepository
    {
        private readonly ApplicationContext _context;

        public PasswordResetRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<PasswordReset> AddAsync(PasswordReset passwordReset)
        {

            var result = await _context.PasswordResets.AddAsync(passwordReset);
            await _context.SaveChangesAsync();
            return passwordReset;
        }

        public async Task<PasswordReset?> GetByResetCodeAsync(string resetCode)
        {
            return await _context.PasswordResets
                .FirstOrDefaultAsync(r => r.ResetCode == resetCode && !r.IsUsed);
        }

        public async Task UpdateAsync(PasswordReset passwordReset)
        {
            _context.PasswordResets.Update(passwordReset);
            await _context.SaveChangesAsync();
        }

    }
}
