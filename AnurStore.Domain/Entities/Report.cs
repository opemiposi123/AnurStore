using AnurStore.Domain.Common.Contracts;
using AnurStore.Domain.Enums;

namespace AnurStore.Domain.Entities;

public class Report : BaseEntity
{
    public string Title { get; set; } = default!;
    public string? Description { get; set; } 
    public DateTime GeneratedDate { get; set; }
    public ReportType ReportType { get; set; }
    public string GeneratedById { get; set; } = default!;
    public User GeneratedBy { get; set; } = default!;
    public string FilePath { get; set; } = default!;
    public string ReportData { get; set; } = default!;
}
