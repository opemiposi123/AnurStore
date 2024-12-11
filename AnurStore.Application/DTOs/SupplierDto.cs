using MassTransit;

namespace AnurStore.Application.DTOs
{
    public class SupplierDto
    {
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Location { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string Id { get; set; } = NewId.Next().ToSequentialGuid().ToString();
        public string CreatedBy { get; set; } = default!;
        public DateTime CreatedOn { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string? DeletedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
