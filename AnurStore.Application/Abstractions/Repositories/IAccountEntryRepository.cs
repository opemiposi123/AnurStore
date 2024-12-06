using AnurStore.Domain.Entities;

namespace AnurStore.Application.Abstractions.Repositories;

public interface IAccountEntryRepository
{
    Task<AccountEntry> CreateAccountEntry(AccountEntry AccountEntry);
    Task<IList<AccountEntry>> GetAllAccountEntrys();
    Task<AccountEntry> GetAccountEntryById(string id);
    Task<bool> UpdateAccountEntry(AccountEntry AccountEntry);
    Task<bool> Exist(string AccountEntryName);
    List<AccountEntry> GetAllAccountEntry();
}
