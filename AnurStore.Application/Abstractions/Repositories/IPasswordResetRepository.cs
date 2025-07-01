using AnurStore.Domain.Entities;

namespace AnurStore.Application.Abstractions.Repositories
{
    public interface IPasswordResetRepository
    {
        Task<PasswordReset> AddAsync(PasswordReset passwordReset);
        Task<PasswordReset?> GetByResetCodeAsync(string resetCode);
        Task UpdateAsync(PasswordReset passwordReset);
    }
}
