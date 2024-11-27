using AnurStore.Domain.Common.Contracts;

namespace AnurStore.Domain.Entities
{
    public class Transaction : BaseEntity
    {
        public DateTime TransactionDate { get; set; } = DateTime.Now;
        public string Reference { get; set; } = default!;
        public string? Description { get; set; } 
        public ICollection<AccountEntry> AccountEntries { get; set; } = new HashSet<AccountEntry>();
    }
}
