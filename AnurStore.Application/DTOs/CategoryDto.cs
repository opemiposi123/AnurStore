using MassTransit;

namespace AnurStore.Application.DTOs
{
    public class CategoryDto
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
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
