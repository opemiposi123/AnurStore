namespace AnurStore.Application.RequestModel
{
    public class UpdateCategoryRequest
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }
}
