using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnurStore.Application.RequestModel
{
    public class PurchaseExportRequest
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? SupplierId { get; set; }
        public string? ProductId { get; set; }
    }
}
