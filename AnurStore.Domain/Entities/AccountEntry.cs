using AnurStore.Domain.Common.Contracts;
using AnurStore.Domain.Enums;

namespace AnurStore.Domain.Entities
{
    public class AccountEntry : BaseEntity
    {
        public string AccountId { get; set; } = default!;
        public Account? Account { get; set; }  // Reference to the account being affected
        public decimal Amount { get; set; }  // The amount being debited or credited
        public EntryType EntryType { get; set; }  // Debit or Credit
        public string TransactionId { get; set; } = default!;// Reference to the parent transaction
        public Transaction? Transaction { get; set; }  // Reference to the parent transaction
    }

}
