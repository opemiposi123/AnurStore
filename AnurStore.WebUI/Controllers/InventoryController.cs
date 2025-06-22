using AnurStore.Application.Abstractions.Services;
using AnurStore.Application.DTOs;
using AnurStore.Application.Services;
using AnurStore.Domain.Enums;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace AnurStore.WebUI.Controllers
{
    public class InventoryController : Controller
    {
        private readonly INotyfService _notyf;
        private readonly IInventoryService _inventoryService; 
        public InventoryController(INotyfService notyf, IInventoryService inventoryService)
        {
            _notyf = notyf;
            _inventoryService = inventoryService;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _inventoryService.GetAllInventories();
            if (response.Status)
            {
                return View(response.Data);
            }
            return View(Enumerable.Empty<InventoryDto>());
        }
        public async Task<IActionResult> GetInventoriesByStatus(StockStatus status) 
        {
            var response = await _inventoryService.GetInventoriesByStatusAsync(status);
            if (response.Status)
            {
                return View(response.Data);
            }
            return View(Enumerable.Empty<InventoryDto>());
        }
    }
}
