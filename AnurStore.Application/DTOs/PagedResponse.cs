namespace AnurStore.Application.DTOs
{
    public class PagedResponse<T>
    {
        public List<T> Data { get; set; } = [];
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public bool HasNextPage => PageNumber * PageSize < TotalRecords;
        public bool HasPreviousPage => PageNumber > 1;
    }

}
