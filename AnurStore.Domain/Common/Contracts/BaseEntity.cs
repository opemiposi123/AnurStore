using MassTransit;

namespace AnurStore.Domain.Common.Contracts
{
    public abstract class BaseEntity : ISoftDelete, IAuditableEntity
    {
        public string Id { get; set; } = NewId.Next().ToSequentialGuid().ToString();
        public string? CreatedBy { get; set; } = default!;  
        public DateTime CreatedOn { get; set; }    
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public DateTime? DeletedOn { get; set; } 
        public string? DeletedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
