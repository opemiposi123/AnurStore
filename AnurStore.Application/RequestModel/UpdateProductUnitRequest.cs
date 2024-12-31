namespace AnurStore.Application.RequestModel
{
    public class UpdateProductUnitRequest
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }
}
