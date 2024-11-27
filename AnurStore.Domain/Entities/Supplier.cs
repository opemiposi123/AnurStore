using AnurStore.Domain.Common.Contracts;

namespace AnurStore.Domain.Entities
{
    public class Supplier : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Location { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
    }
}