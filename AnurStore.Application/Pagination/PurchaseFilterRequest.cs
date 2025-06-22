using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnurStore.Application.Pagination
{
    public class PurchaseFilterRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string? SupplierId { get; set; }
        public string? ProductId { get; set; }
    }

}
