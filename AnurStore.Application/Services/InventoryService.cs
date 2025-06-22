using AnurStore.Application.Abstractions.Repositories;
using AnurStore.Application.Abstractions.Services;
using AnurStore.Application.DTOs;
using AnurStore.Application.Wrapper;
using AnurStore.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace AnurStore.Application.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly ILogger<InventoryService> _logger;
        public InventoryService(IInventoryRepository inventoryRepository, ILogger<InventoryService> logger)
        {
            _inventoryRepository = inventoryRepository;
            _logger = logger;
        }
         
        public async Task<BaseResponse<IEnumerable<InventoryDto>>> GetAllInventories()
        {
            _logger.LogInformation("Starting GetAllInventory method."); 
            try
            {
                var result = await _inventoryRepository.GetAllInventories(); 
                var inventoryDtos = result.Select(r => new InventoryDto
                {
                    Id = r.Id,
                    ProductName = r.Product.Name,
                    ProductBrand = r.Product.Brand.Name,
                    ProductCategory = r.Product.Category.Name,
                    ProductPrice = r.Product?.PricePerPack,
                    QuantityAvailable = r.QuantityAvailable,
                    StockStatus = r.StockStatus,
                    ProductSize = r.Product.ProductSize.Size

                }).ToList();

                _logger.LogInformation("Successfully retrieved inventories."); 
                return new BaseResponse<IEnumerable<InventoryDto>> 
                {
                    Message = "Record Found Successfully",
                    Status = true,
                    Data = inventoryDtos,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving categories.");
                return new BaseResponse<IEnumerable<InventoryDto>>
                {
                    Status = false,
                    Message = "Failed",
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<InventoryDto>>> GetInventoriesByStatusAsync(StockStatus status)
        {
            var inventories = await _inventoryRepository.GetByStockStatusAsync(status);

            if (inventories == null || !inventories.Any())
            {
                return new BaseResponse<IEnumerable<InventoryDto>>
                {
                    Status = false,
                    Message = $"No inventory records found with status: {status}",
                    Data = new List<InventoryDto>()
                };
            }

            var dtos = inventories.Select(i => new InventoryDto
            {
                Id = i.Id,
                ProductName = i.Product.Name,
                ProductBrand = i.Product.Brand.Name,
                ProductPrice = i.Product?.PricePerPack,
                ProductCategory = i.Product.Category.Name,
                QuantityAvailable = i.QuantityAvailable,
                StockStatus = i.StockStatus,
                ProductSize = i.Product.ProductSize.Size
            });

            return new BaseResponse<IEnumerable<InventoryDto>>
            {
                Status = true,
                Message = "Inventories retrieved successfully",
                Data = dtos
            };
        }

    }
}
