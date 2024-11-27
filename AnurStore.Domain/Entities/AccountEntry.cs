using AnurStore.Domain.Common.Contracts;
using AnurStore.Domain.Enums;

namespace AnurStore.Domain.Entities
{
    public class AccountEntry : BaseEntity
    {
        public string AccountId { get; set; } = default!;
        public Account? Account { get; set; }  
        public decimal Amount { get; set; }  
        public EntryType EntryType { get; set; }  
        public string TransactionId { get; set; } = default!;
        public Transaction? Transaction { get; set; }  
    }
}
