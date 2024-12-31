using AnurStore.Domain.Common.Contracts;

namespace AnurStore.Domain.Entities
{
    public class ProductUnit : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; } 
    }
}