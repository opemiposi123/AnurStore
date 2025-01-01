using AnurStore.Application.Abstractions.Repositories;
using AnurStore.Application.Abstractions.Services;
using AnurStore.Application.DTOs;
using AnurStore.Application.RequestModel;
using AnurStore.Application.Wrapper;
using AnurStore.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AnurStore.Application.Services
{
    public class ProductUnitService : IProductUnitService
    {
        private readonly IProductUnitRepository _productUnitRepository;
        private readonly ILogger<ProductUnitService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductUnitService(IProductUnitRepository productUnitRepository, ILogger<ProductUnitService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _productUnitRepository = productUnitRepository;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse<string>> CreateProductUnit(CreateProductUnitRequest request)
        {
            var userName = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "System";
            _logger.LogInformation("Starting CreateProductUnit method.");
            try
            {
                if (request == null)
                {
                    _logger.LogWarning("CreateProductUnit request is null.");
                    return new BaseResponse<string>
                    {
                        Message = "Request cannot be null",
                        Status = false,
                    };
                }
                _logger.LogInformation("Checking if ProductUnit with name {ProductUnitName} exists.", request.Name);
                var reportExists = await _productUnitRepository.Exist(request.Name);

                if (reportExists)
                {
                    _logger.LogWarning("ProductUnit with name {ProductUnitName} already exists.", request.Name);
                    return new BaseResponse<string>
                    {
                        Status = false,
                        Message = "ProductUnit name already exists"
                    };
                }

                var ProductUnit = new ProductUnit
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = request.Name,
                    Description = request.Description,
                    CreatedBy = userName,
                    CreatedOn = DateTime.Now
                };

                _logger.LogInformation("Creating new ProductUnit {ProductUnitName}.", request.Name);
                var createdProductUnit = await _productUnitRepository.CreateProductUnit(ProductUnit);

                _logger.LogInformation("Report type created successfully with Id {ProductUnitId}.", ProductUnit.Id);
                return new BaseResponse<string>
                {
                    Message = "ProductUnit created successfully",
                    Status = true,
                    Data = ProductUnit.Id,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating ProductUnit.");
                return new BaseResponse<string>
                {
                    Message = $"Failed to create ProductUnit: {ex.Message}",
                    Status = false,
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteProductUnit(string productUnitId)
        {
            try
            {
                var ProductUnit = await _productUnitRepository.GetProductUnitById(productUnitId);
                if (ProductUnit == null)
                {
                    _logger.LogWarning($"ProductUnit with Id {productUnitId} not found.");
                    return new BaseResponse<bool>
                    {
                        Message = $"ProductUnit not found",
                        Status = false,
                    };
                }

                ProductUnit.IsDeleted = true;

                var result = await _productUnitRepository.UpdateProductUnit(ProductUnit);

                if (result)
                {
                    _logger.LogInformation($"ProductUnit with Id {productUnitId} deleted successfully.");
                    return new BaseResponse<bool>
                    {
                        Message = $"ProductUnit deleted successfuly",
                        Status = false,
                    };
                }
                else
                {
                    _logger.LogError($"Failed to delete ProductUnit with Id {productUnitId}.");
                    return new BaseResponse<bool>
                    {
                        Message = $"Fail to delete ProductUnit",
                        Status = false,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting ProductUnit with Id {ProductUnitId}.", productUnitId);
                return new BaseResponse<bool>
                {
                    Message = $"An error occured {ex.Message}",
                    Status = false,
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<ProductUnitDto>>> GetAllProductUnit()
        {
            _logger.LogInformation("Starting GetAllProductUnit method.");
            try
            {
                var report = await _productUnitRepository.GetAllProductUnit();
                var ProductUnitDtos = report.Where(x => !x.IsDeleted).Select(r => new ProductUnitDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description
                }).ToList();

                _logger.LogInformation("Successfully retrieved units.");
                return new BaseResponse<IEnumerable<ProductUnitDto>>
                {
                    Message = "Record Found Successfully",
                    Status = true,
                    Data = ProductUnitDtos,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving categories.");
                return new BaseResponse<IEnumerable<ProductUnitDto>>
                {
                    Status = false,
                    Message = "Failed",
                };
            }
        }

        public async Task<BaseResponse<ProductUnitDto>> GetProductUnit(string productUnitId)
        {
            _logger.LogInformation("Starting GetProductUnit method for Id {ProductUnitId}.", productUnitId);
            try
            {
                var reportType = await _productUnitRepository.GetProductUnitById(productUnitId);
                if (reportType == null)
                {
                    _logger.LogWarning("ProductUnit with Id {ProductUnitId} not found.", productUnitId);
                    return new BaseResponse<ProductUnitDto>
                    {
                        Message = "ProductUnit not found",
                        Status = false
                    };
                }

                var ProductUnitDto = new ProductUnitDto
                {
                    Id = productUnitId,
                    Name = reportType.Name,
                    Description = reportType.Description,
                    CreatedBy = reportType.CreatedBy,
                    LastModifiedBy = reportType.LastModifiedBy,
                    CreatedOn = reportType.CreatedOn,
                    LastModifiedOn = reportType.LastModifiedOn
                };

                _logger.LogInformation("Successfully retrieved ProductUnit with Id {ProductUnitId}.", productUnitId);
                return new BaseResponse<ProductUnitDto>
                {
                    Data = ProductUnitDto,
                    Status = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving ProductUnit with Id {ProductUnitId}.", productUnitId);
                return new BaseResponse<ProductUnitDto>
                {
                    Message = $"Failed to retrieve ProductUnit: {ex.Message}",
                    Status = false
                };
            }
        }

        public async Task<BaseResponse<bool>> UpdateProductUnit(string ProductUnitId, UpdateProductUnitRequest request)
        {
            var userName = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "System";
            _logger.LogInformation("Starting UpdateProductUnit method for Id {ProductUnitId}.", ProductUnitId);
            try
            {
                var existingProductUnit = await _productUnitRepository.GetProductUnitById(ProductUnitId);
                if (existingProductUnit == null)
                {
                    _logger.LogWarning($"ProductUnit with Id {ProductUnitId} not found.");
                    return new BaseResponse<bool>
                    {
                        Message = "ProductUnit not found",
                        Status = false
                    };
                }

                existingProductUnit.Description = request.Description;
                existingProductUnit.Name = request.Name;

                await _productUnitRepository.UpdateProductUnit(existingProductUnit);

                var ProductUnit = new ProductUnit
                {
                    LastModifiedBy = userName,
                    LastModifiedOn = existingProductUnit.LastModifiedOn,
                    Description = existingProductUnit.Description,
                };
                _logger.LogInformation("Successfully updated ProductUnit with Id {ProductUnitId}.", ProductUnitId);
                return new BaseResponse<bool>
                {
                    Data = true,
                    Message = "ProductUnit updated successfully",
                    Status = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating ProductUnit with Id {ProductUnitId}.", ProductUnitId);
                return new BaseResponse<bool>
                {
                    Message = $"Failed to update ProductUnit: {ex.Message}",
                    Status = false
                };
            }
        }
    }
}
