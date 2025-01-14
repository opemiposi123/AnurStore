using AnurStore.Application.Abstractions.Repositories;
using AnurStore.Application.Abstractions.Services;
using AnurStore.Application.DTOs;
using AnurStore.Application.RequestModel;
using AnurStore.Application.Wrapper;
using AnurStore.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace AnurStore.Application.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly ILogger<CategoryService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SupplierService(ISupplierRepository supplierRepository, ILogger<CategoryService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _supplierRepository = supplierRepository;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse<string>> CreateSupplier(CreateSupplierRequest request)
        {
            var userName = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "System";
            _logger.LogInformation("Starting CreateSupplier method.");
            try
            {
                if (request == null)
                {
                    _logger.LogWarning("CreateSupplier request is null.");
                    return new BaseResponse<string>
                    {
                        Message = "Request cannot be null",
                        Status = false,
                    };
                }
                _logger.LogInformation("Checking if Supplier with name {SupplierName} exists.", request.Name);
                var supplierExists = await _supplierRepository.Exist(request.Name);

                if (supplierExists)
                {
                    _logger.LogWarning("Supplier with name {SupplierName} already exists.", request.Name);
                    return new BaseResponse<string>
                    {
                        Status = false,
                        Message = "Supplier name already exists"
                    };
                }

                var supplier = new Supplier
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = request.Name,
                    PhoneNumber = request.PhoneNumber,
                    Location = request.Location,
                    Email = request.Email,
                    CreatedBy = userName,
                    CreatedOn = DateTime.Now
                };

                _logger.LogInformation("Creating new Supplier {SupplierName}.", request.Name);
                var createdSupplier = await _supplierRepository.CreateSupplier(supplier);

                _logger.LogInformation("Report type created successfully with Id {SupplierId}.", supplier.Id);
                return new BaseResponse<string>
                {
                    Message = "Supplier created successfully",
                    Status = true,
                    Data = supplier.Id,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating Supplier.");
                return new BaseResponse<string>
                {
                    Message = $"Failed to create Supplier: {ex.Message}",
                    Status = false,
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteSupplier(string supplierId)
        {
            try
            {
                var Supplier = await _supplierRepository.GetSupplierById(supplierId);
                if (Supplier == null)
                {
                    _logger.LogWarning($"Supplier with Id {supplierId} not found.");
                    return new BaseResponse<bool>
                    {
                        Message = $"Supplier not found",
                        Status = false,
                    };
                }

                Supplier.IsDeleted = true;

                var result = await _supplierRepository.UpdateSupplier(Supplier);

                if (result)
                {
                    _logger.LogInformation($"Supplier with Id {supplierId} deleted successfully.");
                    return new BaseResponse<bool>
                    {
                        Message = $"Supplier deleted successfuly",
                        Status = false,
                    };
                }
                else
                {
                    _logger.LogError($"Failed to delete Supplier with Id {supplierId}.");
                    return new BaseResponse<bool>
                    {
                        Message = $"Fail to delete Supplier",
                        Status = false,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting Supplier with Id {SupplierId}.", supplierId);
                return new BaseResponse<bool>
                {
                    Message = $"An error occured {ex.Message}",
                    Status = false,
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<SupplierDto>>> GetAllSupplier()
        {
            _logger.LogInformation("Starting GetAllSupplier method.");
            try
            {
                var supplier =  await _supplierRepository.GetAllSuppliers();
                var SupplierDtos = supplier.Where(x => !x.IsDeleted).Select(r => new SupplierDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Email = r.Email,
                    PhoneNumber = r.PhoneNumber,
                    Location = r.Location
                }).ToList();

                _logger.LogInformation("Successfully retrieved suppliers.");
                return new BaseResponse<IEnumerable<SupplierDto>>
                {
                    Message = "Record Found Successfully",
                    Status = true,
                    Data = SupplierDtos,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving suppliers.");
                return new BaseResponse<IEnumerable<SupplierDto>>
                {
                    Status = false,
                    Message = "Failed",
                };
            }
        }

        public async Task<BaseResponse<SupplierDto>> GetSupplier(string supplierId)
        {
            _logger.LogInformation("Starting GetSupplier method for Id {SupplierId}.", supplierId);
            try
            {
                var supplier = await _supplierRepository.GetSupplierById(supplierId);
                if (supplier == null)
                {
                    _logger.LogWarning("Supplier with Id {SupplierId} not found.", supplierId);
                    return new BaseResponse<SupplierDto>
                    {
                        Message = "Supplier not found",
                        Status = false
                    };
                }

                var supplierDto = new SupplierDto
                {
                    Id = supplierId,
                    Name = supplier.Name,
                    Email = supplier.Email,
                    PhoneNumber = supplier.PhoneNumber,
                    Location = supplier.Location,
                    CreatedBy = supplier.CreatedBy,
                    LastModifiedBy = supplier.LastModifiedBy,
                    CreatedOn = supplier.CreatedOn,
                    LastModifiedOn = supplier.LastModifiedOn
                };

                _logger.LogInformation("Successfully retrieved Supplier with Id {SupplierId}.", supplierId);
                return new BaseResponse<SupplierDto>
                {
                    Data = supplierDto,
                    Status = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving Supplier with Id {SupplierId}.", supplierId);
                return new BaseResponse<SupplierDto>
                {
                    Message = $"Failed to retrieve Supplier: {ex.Message}",
                    Status = false
                };
            }
        }

        public async Task<BaseResponse<bool>> UpdateSupplier(string supplierId, UpdateSupplierRequest request)
        {
            var userName = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "System";
            _logger.LogInformation("Starting UpdateSupplier method for Id {SupplierId}.", supplierId);
            try
            {
                var existingSupplier = await _supplierRepository.GetSupplierById(supplierId);
                if (existingSupplier == null)
                {
                    _logger.LogWarning($"Supplier with Id {supplierId} not found.");
                    return new BaseResponse<bool>
                    {
                        Message = "Supplier not found",
                        Status = false
                    };
                }

                existingSupplier.Email = request.Email;
                existingSupplier.Location = request.Location;
                existingSupplier.PhoneNumber = request.PhoneNumber; 
                existingSupplier.Name = request.Name;

                await _supplierRepository.UpdateSupplier(existingSupplier);
                _logger.LogInformation("Successfully updated Supplier with Id {SupplierId}.", supplierId);
                return new BaseResponse<bool>
                {
                    Data = true,
                    Message = "Supplier updated successfully",
                    Status = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating Supplier with Id {SupplierId}.", supplierId);
                return new BaseResponse<bool>
                {
                    Message = $"Failed to update Supplier: {ex.Message}",
                    Status = false
                };
            }
        }

        public async Task<IEnumerable<SelectListItem>> GetSupplierSelectList()
        {
            var supplierResponse = await GetAllSupplier(); 
            if (supplierResponse.Status && supplierResponse.Data != null)
            {
                var supplierList = supplierResponse.Data.Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name
                });
                return supplierList;
            }
            return Enumerable.Empty<SelectListItem>();
        }
    }
}
