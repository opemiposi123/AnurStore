using AnurStore.Domain.Common.Contracts;
using AnurStore.Domain.Enums;

namespace AnurStore.Domain.Entities
{
    public class Report : BaseEntity
    {
        public string Title { get; set; } 
        public string? Description { get; set; } 
        public DateTime GeneratedDate { get; set; } = DateTime.Now; 
        public ReportType ReportType { get; set; } 
        public string GeneratedById { get; set; } 
        public User GeneratedBy { get; set; } 
        public string FilePath { get; set; } 
        public string ReportData { get; set; } 
    }

}
