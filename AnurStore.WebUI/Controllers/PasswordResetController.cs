using AnurStore.Application.Abstractions.Services;
using AnurStore.Application.DTOs;
using AnurStore.Application.RequestModel;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace AnurStore.WebUI.Controllers
{
    public class PasswordResetController : Controller
    {
        private readonly IPasswordResetService _passwordResetService;
        private readonly INotyfService _notyf;

        public PasswordResetController(IPasswordResetService passwordResetService, INotyfService notyf)
        {
            _passwordResetService = passwordResetService;
            _notyf = notyf;
        }

        [HttpGet("enter-reset-code")]
        public IActionResult EnterResetCode()
        {
            return View();
        }

        [HttpPost("enter-reset-code")]
        public async Task<IActionResult> EnterResetCode(ValidateResetCode request)
        {
            var response = await _passwordResetService.ValidateResetCodeAsync(request);

            if (response.Status)
            {
                _notyf.Success(response.Message, 3);
                return RedirectToAction("Login", "UserAuth");;
            }

            _notyf.Error(response.Message);
            return View();
        }

        [HttpGet("request-password-reset")]
        public IActionResult RequestPasswordReset()
        {
            return View();
        }

        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestPasswordReset(CreateNewPassWord request)
        {
            var response = await _passwordResetService.CreateNewPassWord(request);

            if (response.Status)
            {
                _notyf.Success(response.Message, 3);
                return RedirectToAction("EnterResetCode");
            }

            _notyf.Error(response.Message);
            return View();
        }
    }
}
