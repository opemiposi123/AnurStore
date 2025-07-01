using AnurStore.Application.Abstractions.Repositories;
using AnurStore.Application.Abstractions.Services;
using AnurStore.Application.DTOs;
using AnurStore.Application.Helper;
using AnurStore.Application.Pagination;
using AnurStore.Application.RequestModel;
using AnurStore.Application.Wrapper;
using AnurStore.Domain.Entities;
using AnurStore.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;

namespace AnurStore.Application.Services
{
    public class ProductPurchaseService : IProductPurchaseService
    {
        private readonly IInventoryRepository _inventoryRepo;
        private readonly IProductRepository _productRepo;
        private readonly IProductPurchaseRepository _productpurchaseRepo;
        private readonly IProductService _productService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<ProductPurchaseService> _logger;

        public ProductPurchaseService(IInventoryRepository inventoryRepo,
            IProductRepository productRepo,
            IProductPurchaseRepository productPurchaseRepo,
            IProductService productService,
            ILogger<ProductPurchaseService> logger,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor,
             UserManager<User> userManager)
        {
            _inventoryRepo = inventoryRepo;
            _productRepo = productRepo;
            _productpurchaseRepo = productPurchaseRepo;
            _productService = productService;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _logger = logger;
        }
        public async Task<BaseResponse<string>> PurchaseProductsAsync(CreateProductPurchaseRequest request, string userName)
        {
            _logger.LogInformation("Creating product purchase. SupplierId: {SupplierId}, Batch: {Batch}", request.SupplierId, request.Batch);
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var purchase = new ProductPurchase
                {
                    Batch = request.Batch, 
                    SupplierId = request.SupplierId,
                    Discount = request.Discount,
                    Total = request.Total,
                    PurchaseDate = request.PurchaseDate,
                    IsAddedToInventory = false,
                    CreatedBy = userName,
                    CreatedOn = DateTime.Now,
                    PurchaseItems = request.PurchaseItems.Select(i => new ProductPurchaseItem
                    {
                        ProductId = i.ProductId,
                        Quantity = i.Quantity,
                        Rate = i.Rate,
                        TotalCost = i.TotalCost,
                        CreatedBy = userName,
                        CreatedOn = DateTime.Now
                    }).ToList()
                };

                await _productpurchaseRepo.PurchaseProductAsync(purchase);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();

                _logger.LogInformation("Purchase saved. ID: {Id}", purchase.Id);

                return new BaseResponse<string>
                {
                    Status = true,
                    Message = "Purchase created successfully",
                    Data = purchase.Id
                };
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                _logger.LogError(ex, "Error saving purchase. SupplierId: {SupplierId}, Batch: {Batch}", request.SupplierId, request.Batch);

                return new BaseResponse<string>
                {
                    Status = false,
                    Message = $"Failed to save purchase: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponse<string>> ProcessInventoryAndProductUpdateAsync(string purchaseId, string userName)
        {
            _logger.LogInformation("Processing inventory/product updates for PurchaseId: {PurchaseId}", purchaseId);
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var purchase = await _productpurchaseRepo.GetByIdWithItemsAsync(purchaseId);
                if (purchase == null)
                {
                    return new BaseResponse<string>
                    {
                        Status = false,
                        Message = "Purchase not found."
                    };
                }

                var inventoryToAdd = new List<Inventory>();
                var productToUpdate = new List<Product>();

                foreach (var item in purchase.PurchaseItems)
                {
                    var product = await _productRepo.GetProductById(item.ProductId);
                    if (product == null)
                        continue;

                    int totalPieces = item.Quantity * product.TotalItemInPack;

                    var inventory = await _inventoryRepo.GetByProductAsync(item.ProductId);
                    if (inventory != null)
                    {
                        inventory.TotalPiecesAvailable += totalPieces;
                        inventory.StockDate = DateTime.Now;
                        await _inventoryRepo.UpdateAsync(inventory);
                    }
                    else
                    {
                        inventoryToAdd.Add(new Inventory
                        {
                            ProductId = item.ProductId,
                            TotalPiecesAvailable = totalPieces,
                            StockDate = DateTime.Now,
                            BatchNumber = purchase.Batch,
                            StockStatus = StockStatus.InStock,
                            Remark = "Restocked",
                            CreatedBy = userName,
                            CreatedOn = DateTime.Now
                        });
                    }

                    var newPackPrice = PricingCalculator.CalculatePackSellingPrice(item.Rate, product.PackPriceMarkup);
                    var unitsPerPack = product.TotalItemInPack;
                    var newUnitPrice = unitsPerPack > 0
                        ? PricingCalculator.CalculateUnitSellingPrice(newPackPrice, unitsPerPack)
                        : 0;

                    product.PricePerPack = newPackPrice;
                    product.UnitPrice = newUnitPrice;

                    productToUpdate.Add(product);
                }

                foreach (var inv in inventoryToAdd)
                    await _inventoryRepo.AddAsync(inv);

                foreach (var p in productToUpdate)
                    await _productRepo.UpdateProduct(p);

                // ✅ Mark purchase as processed
                purchase.IsAddedToInventory = true;
                await _productpurchaseRepo.UpdateAsync(purchase);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();

                _logger.LogInformation("Inventory/product updates processed for PurchaseId: {PurchaseId}", purchaseId);
                return new BaseResponse<string>
                {
                    Status = true,
                    Message = "Inventory and product updates processed successfully.",
                };
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                _logger.LogError(ex, "Failed to process updates for PurchaseId: {PurchaseId}", purchaseId);
                return new BaseResponse<string>
                {
                    Status = false,
                    Message = $"Error: {ex.Message}",
                };
            }
        }


        public async Task<BaseResponse<bool>> DeletePurchaseAsync(string purchaseId)
        {
            try
            {
                var purchase = await _productpurchaseRepo.GetByIdAsync(purchaseId);
                if (purchase == null)
                {
                    _logger.LogWarning($"Purchase with Id {purchaseId} not found.");
                    return new BaseResponse<bool>
                    {
                        Message = $"Brand not found",
                        Status = false,
                    };
                }

                purchase.IsDeleted = true;

                var result = await _productpurchaseRepo.UpdateAsync(purchase);

                if (result)
                {
                    _logger.LogInformation($"Purchase with Id {purchaseId} deleted successfully.");
                    return new BaseResponse<bool>
                    {
                        Message = $"Purchase deleted successfuly",
                        Status = false,
                    };
                }
                else
                {
                    _logger.LogError($"Failed to delete Purchase with Id {purchaseId}.");
                    return new BaseResponse<bool>
                    {
                        Message = $"Fail to delete Purchase",
                        Status = false,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting Purchase with Id {BrandId}.", purchaseId);
                return new BaseResponse<bool>
                {
                    Message = $"An error occured {ex.Message}",
                    Status = false,
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<ProductPurchaseDto>>> GetAllPurchasesAsync()
        {
            _logger.LogInformation("Starting GetAllProductPurchase method.");
            try
            {
                var userPrincipal = _httpContextAccessor.HttpContext?.User;
                if (userPrincipal == null)
                {
                    return new BaseResponse<IEnumerable<ProductPurchaseDto>>
                    {
                        Status = false,
                        Message = "User context not found."
                    };
                }

                var user = await _userManager.GetUserAsync(userPrincipal);
                if (user == null)
                {
                    return new BaseResponse<IEnumerable<ProductPurchaseDto>>
                    {
                        Status = false,
                        Message = "User not found."
                    };
                }

                bool isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
                var username = userPrincipal?.Identity?.Name;
                var response = await _productpurchaseRepo.GetAllAsync(username);
                var productPurchaseeDtos = response.Where(x => !x.IsDeleted).Select(r => new ProductPurchaseDto
                {
                    Id = r.Id,
                    SupplierName = r.Supplier.Name,
                    Total = r.Total,
                    Discount = r.Discount,
                    PurchaseDate = r.PurchaseDate,
                    Batch = r.Batch,
                    CreatedBy = r.CreatedBy
                }).ToList();

                _logger.LogInformation("Successfully retrieved product purchases.");
                return new BaseResponse<IEnumerable<ProductPurchaseDto>>
                {
                    Message = "Record Found Successfully",
                    Status = true,
                    Data = productPurchaseeDtos,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving product purchases.");
                return new BaseResponse<IEnumerable<ProductPurchaseDto>>
                {
                    Status = false,
                    Message = "Failed",
                };
            }
        }

        public async Task<BaseResponse<ProductPurchaseDto>> GetPurchaseDetailsAsync(string purchaseId)
        {
            var purchase = await _productpurchaseRepo.GetByIdAsync(purchaseId);

            if (purchase == null)
            {
                return new BaseResponse<ProductPurchaseDto>
                {
                    Status = false,
                    Message = "Purchase not found"
                };
            }

            var dto = new ProductPurchaseDto
            {
                Id = purchase.Id,
                Batch = purchase.Batch,
                Total = purchase.Total,
                Discount = purchase.Discount,
                PurchaseDate = purchase.PurchaseDate,
                SupplierName = purchase.Supplier.Name,
                IsAddedToInventory = purchase.IsAddedToInventory,
                CreatedBy = purchase.CreatedBy,
                PurchaseItems = purchase.PurchaseItems.Select(item => new ProductPurchaseItemDto
                {
                    ProductName = item.Product.Name,
                    Quantity = item.Quantity,
                    Rate = item.Rate,
                    TotalCost = item.TotalCost
                }).ToList()
            };

            return new BaseResponse<ProductPurchaseDto>
            {
                Status = true,
                Data = dto,
                Message = "Purchase details retrieved successfully"
            };
        }

        public async Task<BaseResponse<IEnumerable<ProductPurchaseDto>>> GetPurchasesBySupplierAsync(string supplierId)
        {
            var purchases = await _productpurchaseRepo.GetBySupplierIdAsync(supplierId);

            if (purchases == null || !purchases.Any())
            {
                return new BaseResponse<IEnumerable<ProductPurchaseDto>>
                {
                    Status = false,
                    Message = "No purchases found for this supplier"
                };
            }

            var dtos = purchases.Select(p => new ProductPurchaseDto
            {
                Id = p.Id,
                Batch = p.Batch,
                Total = p.Total,
                Discount = p.Discount,
                PurchaseDate = p.PurchaseDate,
                SupplierName = p.Supplier.Name,
                IsAddedToInventory = p.IsAddedToInventory
            }).ToList();

            return new BaseResponse<IEnumerable<ProductPurchaseDto>>
            {
                Status = true,
                Data = dtos,
                Message = "Purchases retrieved successfully"
            };
        }

        public async Task<BaseResponse<IEnumerable<ProductPurchaseDto>>> GetPurchasesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var purchases = await _productpurchaseRepo.GetByDateRangeAsync(startDate, endDate);

            if (purchases == null || !purchases.Any())
            {
                return new BaseResponse<IEnumerable<ProductPurchaseDto>>
                {
                    Status = false,
                    Message = "No purchases found for the selected date range."
                };
            }

            var dtos = purchases.Select(p => new ProductPurchaseDto
            {
                Id = p.Id,
                Batch = p.Batch,
                Total = p.Total,
                Discount = p.Discount,
                PurchaseDate = p.PurchaseDate,
                IsAddedToInventory = p.IsAddedToInventory,
                SupplierName = p.Supplier.Name
            }).ToList();

            return new BaseResponse<IEnumerable<ProductPurchaseDto>>
            {
                Status = true,
                Message = "Purchases retrieved successfully",
                Data = dtos
            };
        }

        public async Task<BaseResponse<IEnumerable<ProductPurchaseDto>>> GetPurchasesByProductAsync(string productId)
        {
            var purchases = await _productpurchaseRepo.GetPurchasesByProductAsync(productId);

            if (purchases == null || !purchases.Any())
            {
                return new BaseResponse<IEnumerable<ProductPurchaseDto>>
                {
                    Status = false,
                    Message = "No purchases found for this product",
                    Data = Enumerable.Empty<ProductPurchaseDto>()
                };
            }

            var dtos = purchases.Select(p => new ProductPurchaseDto
            {
                Id = p.Id,
                Batch = p.Batch,
                Total = p.Total,
                Discount = p.Discount,
                PurchaseDate = p.PurchaseDate,
                IsAddedToInventory = p.IsAddedToInventory,
                SupplierName = p.Supplier?.Name ?? "N/A"
            }).ToList();

            return new BaseResponse<IEnumerable<ProductPurchaseDto>>
            {
                Status = true,
                Message = "Purchases retrieved successfully",
                Data = dtos
            };
        }

        public async Task<BaseResponse<byte[]>> ExportPurchasesToExcelAsync(PurchaseExportRequest request)
        {
            var purchases = await _productpurchaseRepo.GetAllWithDetailsAsync();

            // Apply filters
            if (request.StartDate.HasValue)
                purchases = purchases.Where(p => p.PurchaseDate >= request.StartDate.Value).ToList();

            if (request.EndDate.HasValue)
                purchases = purchases.Where(p => p.PurchaseDate <= request.EndDate.Value).ToList();

            if (!string.IsNullOrEmpty(request.SupplierId))
                purchases = purchases.Where(p => p.SupplierId == request.SupplierId).ToList();

            if (!string.IsNullOrEmpty(request.ProductId))
                purchases = purchases
                    .Where(p => p.PurchaseItems.Any(i => i.ProductId == request.ProductId))
                    .ToList();

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Purchases");

            // Headers
            worksheet.Cells[1, 1].Value = "Purchase ID";
            worksheet.Cells[1, 2].Value = "Supplier";
            worksheet.Cells[1, 3].Value = "Batch";
            worksheet.Cells[1, 4].Value = "Total";
            worksheet.Cells[1, 5].Value = "Discount";
            worksheet.Cells[1, 6].Value = "Date";
            worksheet.Cells[1, 7].Value = "Product(s)";

            int row = 2;
            foreach (var p in purchases)
            {
                worksheet.Cells[row, 1].Value = p.Id;
                worksheet.Cells[row, 2].Value = p.Supplier?.Name ?? "N/A";
                worksheet.Cells[row, 3].Value = p.Batch;
                worksheet.Cells[row, 4].Value = p.Total;
                worksheet.Cells[row, 5].Value = p.Discount;
                worksheet.Cells[row, 6].Value = p.PurchaseDate.ToString("yyyy-MM-dd");
                worksheet.Cells[row, 7].Value = string.Join(", ", p.PurchaseItems.Select(i => i.Product.Name));

                row++;
            }

            worksheet.Cells.AutoFitColumns();

            var fileBytes = await package.GetAsByteArrayAsync();

            return new BaseResponse<byte[]>
            {
                Status = true,
                Message = "Excel file generated successfully",
                Data = fileBytes
            };
        }

        public async Task<BaseResponse<PaginatedResult<ProductPurchaseDto>>> GetPurchasesPagedAsync(PurchaseFilterRequest filter)
        {
            var (purchases, totalCount) = await _productpurchaseRepo.GetPagedPurchasesAsync(filter);

            if (purchases == null || !purchases.Any())
            {
                return new BaseResponse<PaginatedResult<ProductPurchaseDto>>
                {
                    Status = false,
                    Message = "No purchases found",
                    Data = new PaginatedResult<ProductPurchaseDto>
                    {
                        Items = new List<ProductPurchaseDto>(),
                        TotalCount = 0,
                        PageNumber = filter.PageNumber,
                        PageSize = filter.PageSize
                    }
                };
            }

            var dtos = purchases.Select(p => new ProductPurchaseDto
            {
                Id = p.Id,
                Batch = p.Batch,
                Total = p.Total,
                Discount = p.Discount,
                PurchaseDate = p.PurchaseDate,
                IsAddedToInventory = p.IsAddedToInventory,
                SupplierName = p.Supplier?.Name ?? "N/A"
            }).ToList();

            return new BaseResponse<PaginatedResult<ProductPurchaseDto>>
            {
                Status = true,
                Message = "Paged purchases retrieved successfully",
                Data = new PaginatedResult<ProductPurchaseDto>
                {
                    Items = dtos,
                    TotalCount = totalCount,
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize
                }
            };
        }

      
    }

}
