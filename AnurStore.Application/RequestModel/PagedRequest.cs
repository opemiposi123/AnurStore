namespace AnurStore.Application.RequestModel
{
    public class PagedRequest
    {
        public int PageNumber { get; set; } = 1;  // Default to first page
        public int PageSize { get; set; } = 10;   // Default to 10 records per page
    }

}
