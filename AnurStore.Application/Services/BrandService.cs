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
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly ILogger<CategoryService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BrandService(IBrandRepository categoryRepository, ILogger<CategoryService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _brandRepository = categoryRepository;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse<string>> CreateBrand(CreateBrandRequest request)
        {
            var userName = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "System";
            _logger.LogInformation("Starting CreateBrand method.");
            try
            {
                if (request == null)
                {
                    _logger.LogWarning("CreateBrand request is null.");
                    return new BaseResponse<string>
                    {
                        Message = "Request cannot be null",
                        Status = false,
                    };
                }

                _logger.LogInformation("Checking if Brand with name {BrandName} exists.", request.Name);
                var brandExists = await _brandRepository.Exist(request.Name);

                if (brandExists)
                {
                    _logger.LogWarning("Brand with name {BrandName} already exists.", request.Name);
                    return new BaseResponse<string>
                    {
                        Status = false,
                        Message = "Brand already exists"
                    };
                }

                var brand = new Brand
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = request.Name,
                    Description = request.Description,
                    CreatedBy = userName,
                    CreatedOn = DateTime.Now
                };

                _logger.LogInformation("Creating new Brand {BrandName}.", request.Name);
                var createdBrand = await _brandRepository.CreateBrand(brand);

                _logger.LogInformation("Report type created successfully with Id {BrandId}.", brand.Id);
                return new BaseResponse<string>
                {
                    Message = "Brand created successfully",
                    Status = true,
                    Data = brand.Id,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating Brand.");
                return new BaseResponse<string>
                {
                    Message = $"Failed to create Brand: {ex.Message}",
                    Status = false,
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteBrand(string brandId)
        {
            try
            {
                var brand = await _brandRepository.GetBrandById(brandId);
                if (brand == null)
                {
                    _logger.LogWarning($"Brand with Id {brandId} not found.");
                    return new BaseResponse<bool>
                    {
                        Message = $"Brand not found",
                        Status = false,
                    };
                }

                brand.IsDeleted = true;

                var result = await _brandRepository.UpdateBrand(brand);

                if (result)
                {
                    _logger.LogInformation($"Brand with Id {brandId} deleted successfully.");
                    return new BaseResponse<bool>
                    {
                        Message = $"Brand deleted successfuly",
                        Status = false,
                    };
                }
                else
                {
                    _logger.LogError($"Failed to delete Brand with Id {brandId}.");
                    return new BaseResponse<bool>
                    {
                        Message = $"Fail to delete Brand",
                        Status = false,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting Brand with Id {BrandId}.", brandId);
                return new BaseResponse<bool>
                {
                    Message = $"An error occured {ex.Message}",
                    Status = false,
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<BrandDto>>> GetAllBrand()
        {
            _logger.LogInformation("Starting GetAllBrand method.");
            try 
            { 
                var result = await _brandRepository.GetAllBrands();
                var brandDtos = result.Select(r => new BrandDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description
                }).ToList();

                _logger.LogInformation("Successfully retrieved brands.");
                return new BaseResponse<IEnumerable<BrandDto>>
                {
                    Message = "Record Found Successfully",
                    Status = true,
                    Data = brandDtos,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving categories.");
                return new BaseResponse<IEnumerable<BrandDto>>
                {
                    Status = false,
                    Message = "Failed",
                };
            }
        }

        public async Task<BaseResponse<BrandDto>> GetBrand(string brandId) 
        {
            _logger.LogInformation("Starting GetBrand method for Id {BrandId}.", brandId);
            try
            {
                var brand = await _brandRepository.GetBrandById(brandId);
                if (brand == null)
                {
                    _logger.LogWarning("Brand with Id {BrandId} not found.", brandId);
                    return new BaseResponse<BrandDto>
                    {
                        Message = "Brand not found",
                        Status = false
                    };
                }

                var brandDto = new BrandDto
                {
                    Id = brandId,
                    Name = brand.Name,
                    Description = brand.Description,
                    CreatedBy = brand.CreatedBy,
                    LastModifiedBy = brand.LastModifiedBy,
                    CreatedOn = brand.CreatedOn,
                    LastModifiedOn = brand.LastModifiedOn
                };

                _logger.LogInformation("Successfully retrieved Brand with Id {BrandId}.", brandId);
                return new BaseResponse<BrandDto>
                {
                    Data = brandDto,
                    Status = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving Brand with Id {BrandId}.", brandId);
                return new BaseResponse<BrandDto>
                {
                    Message = $"Failed to retrieve Brand: {ex.Message}",
                    Status = false
                };
            }
        }

        public async Task<BaseResponse<bool>> UpdateBrand(string brandId, UpdateBrandRequest request)
        {
            var userName = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "System";
            _logger.LogInformation("Starting UpdateBrand method for Id {BrandId}.", brandId);
            try
            {
                var existingBrand = await _brandRepository.GetBrandById(brandId);
                if (existingBrand == null)
                {
                    _logger.LogWarning($"Brand with Id {brandId} not found.");
                    return new BaseResponse<bool>
                    {
                        Message = "Brand not found",
                        Status = false
                    };
                }

                existingBrand.Description = request.Description;
                existingBrand.Name = request.Name;

                await _brandRepository.UpdateBrand(existingBrand);

                var Brand = new Brand
                {
                    LastModifiedBy = userName,
                    LastModifiedOn = existingBrand.LastModifiedOn,
                    Description = existingBrand.Description,
                };
                _logger.LogInformation("Successfully updated Brand with Id {BrandId}.", brandId);
                return new BaseResponse<bool>
                {
                    Data = true,
                    Message = "Brand updated successfully",
                    Status = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating Brand with Id {BrandId}.", brandId);
                return new BaseResponse<bool>
                {
                    Message = $"Failed to update Brand: {ex.Message}",
                    Status = false
                };
            }
        }

        public async Task<IEnumerable<SelectListItem>> GetBrandSelectList()
        {
            var brandsResponse = await GetAllBrand();
            if (brandsResponse.Status && brandsResponse.Data != null)
            {
                var brandList = brandsResponse.Data.Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name 
                });
                return brandList;
            }
            return Enumerable.Empty<SelectListItem>();
        }

    }
}
