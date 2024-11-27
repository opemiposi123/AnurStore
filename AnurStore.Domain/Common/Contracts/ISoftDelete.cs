namespace AnurStore.Domain.Common.Contracts
{
    public interface ISoftDelete
    {
        public DateTime? DeletedOn { get; set; }
        public string? DeletedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}