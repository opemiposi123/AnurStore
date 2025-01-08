using AnurStore.Application.Abstractions.Repositories;
using AnurStore.Application.Abstractions.Services;
using AnurStore.Application.DTOs;
using AnurStore.Application.RequestModel;
using AnurStore.Application.Wrapper;
using AnurStore.Domain.Entities;
using AnurStore.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AnurStore.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(UserManager<User> userManager,
                              RoleManager<IdentityRole> roleManager,
                              IUserRepository userRepository,
                              ILogger<UserService> logger,
                              IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager; 
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse<string>> AddUser(CreateUserRequest request)
        {
            try
            {
                var userName = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "System";

                var userExists = await _userManager.FindByNameAsync(request.Username);
                if (userExists != null)
                {
                    return new BaseResponse<string>
                    {
                        Message = "User already exists",
                        Status = false
                    };
                }

                var user = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    UserName = request.Username,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    Gender = request.Gender,
                    CreatedOn = DateTime.Now,
                    CreatedBy = userName
                };

                var result = await _userManager.CreateAsync(user, request.Password);
                if (!result.Succeeded)
                {
                    return new BaseResponse<string>
                    {
                        Message = "User creation failed",
                        Status = false
                    };
                }

                await _userManager.AddToRoleAsync(user, Role.Cashier.ToString());
                await _userRepository.SaveChangesAsync();

                _logger.LogInformation("User created a new account with password.");

                return new BaseResponse<string>
                {
                    Message = "User created successfully",
                    Status = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a user.");

                return new BaseResponse<string>
                {
                    Message = "Something went wrong",
                    Status = false
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteUser(string id)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                if (user == null)
                {
                    _logger.LogWarning($"User with Id {id} not found.");
                    return new BaseResponse<bool>
                    {
                        Message = "User not found",
                        Status = false
                    };
                }

                user.IsDeleted = true;
                var result = await _userRepository.UpdateAsync(user);

                if (result)
                {
                    _logger.LogInformation($"User with Id {id} deleted successfully.");
                    return new BaseResponse<bool>
                    {
                        Message = "User deleted successfully",
                        Status = true
                    };
                }

                _logger.LogError($"Failed to delete User with Id {id}.");
                return new BaseResponse<bool>
                {
                    Message = "Failed to delete user",
                    Status = false
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting User with Id {UserId}.", id);
                return new BaseResponse<bool>
                {
                    Message = $"An error occurred: {ex.Message}",
                    Status = false
                };
            }
        }

        public async Task<List<UserDto>> LoadAllUser()
        {
            _logger.LogInformation("Starting LoadAllUser method.");
            try
            {
                var result = await _userRepository.GetAllAsync();

                var userDtos = result.Select(r => new UserDto
                {
                    Id = r.Id,
                    Username = r.UserName,
                    FirstName = r.FirstName, 
                    Role = r.Role,
                    Gender = r.Gender,
                    Email = r.Email,
                    Address = r.Address,
                    PhoneNumber = r.PhoneNumber
                }).ToList();

                _logger.LogInformation("Successfully retrieved Users.");
                return userDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving users.");
                throw; 
            }
        }

        public async Task<UserDto> LoadUserDetail(string userId)
        {
            _logger.LogInformation("Starting LoadUserDetail method for Id {UserId}.", userId);
            try
            {
                var user = await _userRepository.GetByIdAsync(userId); 
                if (user == null)
                {
                    _logger.LogWarning("User with Id {UserId} not found.", userId);
                    return null; 
                }

                var userDto = new UserDto
                {
                    Id = user.Id,
                    Username = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Address = user.Address,
                    PhoneNumber = user.PhoneNumber,
                    Gender = user.Gender,
                    Role = user.Role,
                    CreatedBy = user.CreatedBy,
                    LastModifiedBy = user.LastModifiedBy,
                    CreatedOn = user.CreatedOn,
                    LastModifiedOn = user.LastModifiedOn,
                    IsDeleted = user.IsDeleted,
                    DeletedBy = user.DeletedBy,
                    DeletedOn = user.DeletedOn
                };

                _logger.LogInformation("Successfully retrieved User with Id {UserId}.", userId);
                return userDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving User with Id {UserId}.", userId);
                throw; 
            }
        }

        public async Task<BaseResponse<string>> UpdateUser(string id, UserDto updateMemberDto)
        {
            var userName = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "System";
            _logger.LogInformation("Starting UpdateUser method for Id {UserId}.", id);
            try
            {
                var existingUser = await _userRepository.GetByIdAsync(id); 
                if (existingUser == null)
                {
                    _logger.LogWarning($"User with Id {id} not found.");
                    return new BaseResponse<string>
                    {
                        Message = "User not found",
                        Status = false
                    };
                }

                existingUser.FirstName = updateMemberDto.FirstName;
                existingUser.LastName = updateMemberDto.LastName;
                existingUser.Address = updateMemberDto.Address;
                existingUser.Email = updateMemberDto.Email;
                existingUser.PhoneNumber = updateMemberDto.PhoneNumber;
                existingUser.Gender = updateMemberDto.Gender;
                existingUser.Role = updateMemberDto.Role;
                existingUser.LastModifiedBy = userName;
                existingUser.LastModifiedOn = DateTime.UtcNow;

                var updateResult = await _userRepository.UpdateAsync(existingUser); 

                if (!updateResult)
                {
                    return new BaseResponse<string>
                    {
                        Message = "Failed to update user",
                        Status = false
                    };
                }

                _logger.LogInformation("Successfully updated User with Id {UserId}.", id);
                return new BaseResponse<string>
                {
                    Data = id,
                    Message = "User updated successfully",
                    Status = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating User with Id {UserId}.", id);
                return new BaseResponse<string>
                {
                    Message = $"Failed to update User: {ex.Message}",
                    Status = false
                };
            }
        }
    }
}
