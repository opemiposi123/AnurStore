using AnurStore.Application.Abstractions.Services;
using AnurStore.Application.RequestModel;
using AnurStore.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnurStore.WebUI.Controllers
{
    public class UserAuthController : Controller
    {
        private readonly IUserAuthService _userAuthService;

        public UserAuthController(IUserAuthService userAuthService)
        {
            _userAuthService = userAuthService;
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
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["msg"] = result.Message;
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
    }
}
