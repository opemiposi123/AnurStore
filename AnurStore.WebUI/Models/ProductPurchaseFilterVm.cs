using AnurStore.Application.DTOs;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AnurStore.WebUI.Models
{
    public class ProductPurchaseFilterVm
    {
        // Filters
        public int? SupplierId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // Data for dropdown
        public IEnumerable<SelectListItem> Suppliers { get; set; }

        // The filtered result
        public IEnumerable<ProductPurchaseDto> Purchases { get; set; }
    }
}
