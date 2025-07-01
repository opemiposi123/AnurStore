using AnurStore.Application.Abstractions.Services;
using AnurStore.Application.RequestModel;
using AnurStore.Application.Services;
using AspNetCoreHero.ToastNotification.Abstractions;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnurStore.WebUI.Controllers
{
    public class UserAuthController : Controller
    {
        private readonly IUserAuthService _userAuthService;
        private readonly IUserService _userService;
        private readonly INotyfService _notyf;

        public UserAuthController(IUserAuthService userAuthService, IUserService userService, INotyfService notyf)
        {
            _userAuthService = userAuthService;
            _userService = userService;
           _notyf = notyf;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginRequestModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var result = await _userAuthService.LoginAsync(model);
            if (result.StatusCode == 1)
            {
               // _notyf.Success(result.Message);
                return RedirectToAction("Index", "Home");
            }
            else
            {
              //  _notyf.Error(result.Message);
                return RedirectToAction(nameof(Login));
            }
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _userAuthService.LogoutAsync();
            return RedirectToAction(nameof(Login));
        }

        public async Task<IActionResult> ChangePassword([FromForm] ChangePasswordRequestModel changePasswordModelDto, string username)
        {
            var result = await _userAuthService.ChangePasswordAsync(changePasswordModelDto, username);
            return RedirectToAction(nameof(Login));
        }

        public IActionResult UserRegistration() =>
         View();

        [HttpPost]
        public async Task<IActionResult> UserRegistration(CreateUserRequest model, [FromServices] IValidator<CreateUserRequest> validator)
        {
            if (!User.IsInRole("Admin") || model.Role == null)
            {
                model.Role = Domain.Enums.Role.Cashier;
            }
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(model);
            }
            var response = await _userService.AddUser(model);
            _notyf.Success("User onboarded Succesfully");
            return RedirectToAction("Login", "UserAuth");

        }
    }
}
