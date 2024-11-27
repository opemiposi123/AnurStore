using AnurStore.Domain.Common.Contracts;

namespace AnurStore.Domain.Entities
{
    public class Supplier : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }
        public string PhoneNumber { get; set; }      
    }
}