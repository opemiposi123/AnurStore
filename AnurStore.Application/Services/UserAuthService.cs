using AnurStore.Application.Abstractions.Services;
using AnurStore.Application.RequestModel;
using AnurStore.Application.Wrapper;
using AnurStore.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace AnurStore.Application.Services
{
    public class UserAuthService : IUserAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<UserAuthService> _logger;
        public UserAuthService(UserManager<User> userManager,
                                         SignInManager<User> signInManager,
                                         RoleManager<IdentityRole> roleManager,
                                         ILogger<UserAuthService> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<Status> LoginAsync(LoginRequestModel model)
        {
            var status = new Status();

            try
            {
                _logger.LogInformation("Login attempt for user: {Username}", model.Username);

                var user = await _userManager.FindByNameAsync(model.Username);
                if (user == null)
                {
                    _logger.LogWarning("Login failed for user: {Username}. Reason: User not found.", model.Username);
                    status.StatusCode = 0;
                    status.Message = "Invalid credentials";
                    return status;
                }

                var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, true, true);

                if (signInResult.Succeeded)
                {
                    _logger.LogInformation("Login succeeded for user: {Username}", model.Username);

                    var userRoles = await _userManager.GetRolesAsync(user);

                    var authClaims = new List<Claim>
                    {
                         new Claim(ClaimTypes.Name, user.UserName),
                         new Claim(ClaimTypes.GivenName, user.FirstName),
                         new Claim(ClaimTypes.Email, user.Email),
                         new Claim("RegistrationDate", user.CreatedOn.ToString())
                    };

                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    status.StatusCode = 1;
                    status.Message = "Logged in successfully";
                }
                else if (signInResult.IsLockedOut)
                {
                    _logger.LogWarning("User {Username} is locked out.", model.Username);
                    status.StatusCode = 0;
                    status.Message = "Account is locked. Please try again later.";
                }
                else
                {
                    _logger.LogWarning("Login failed for user: {Username}. Reason: Invalid credentials.", model.Username);
                    status.StatusCode = 0;
                    status.Message = "Invalid credentials";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occurred while processing login for user: {Username}", model.Username);
                status.StatusCode = 0;
                status.Message = "An error occurred while processing your request";
            }

            return status;
        }
        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();

        }
        public async Task<Status> ChangePasswordAsync(ChangePasswordRequestModel model, string username)
        {
            var status = new Status();

            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                status.Message = "User does not exist";
                status.StatusCode = 0;
                return status;
            }
            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                status.Message = "Password has updated successfully";
                status.StatusCode = 1;
            }
            else
            {
                status.Message = "Some error occcured";
                status.StatusCode = 0;
            }
            return status;

        }
    }
}
