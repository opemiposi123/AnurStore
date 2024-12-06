using AnurStore.Domain.Entities;

namespace AnurStore.Application.Abstractions.Repositories
{
    public interface IAccountRepository
    {
        Task<Account> CreateAccount(Account Account);
        Task<IList<Account>> GetAllAccounts();
        Task<Account> GetAccountById(string id);
        Task<bool> UpdateAccount(Account Account);
        Task<bool> Exist(string AccountName);
        List<Account> GetAllAccount();
    }
}
