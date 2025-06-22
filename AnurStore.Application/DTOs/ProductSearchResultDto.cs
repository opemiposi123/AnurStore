namespace AnurStore.Application.DTOs
{
    public class ProductSearchResultDto
    {
        public string Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? ImageUrl { get; set; } = default!;
        public decimal? PricePerPack { get; set; } = default!;
    }

}
