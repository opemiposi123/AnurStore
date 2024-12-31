using AnurStore.Application.Abstractions.Services;
using AnurStore.Application.DTOs;
using AnurStore.Application.RequestModel;
using AnurStore.Application.Services;
using AspNetCoreHero.ToastNotification.Abstractions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace AnurStore.WebUI.Controllers
{
    public class UserController : Controller
    {
        private readonly INotyfService _notyf;
        private readonly IUserService _userService;
        public UserController(INotyfService notyf, IUserService userService)
        {
            _notyf = notyf;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _userService.LoadAllUser();
                return View(response);
        }

        public async Task<IActionResult> ViewUserDetail(string id)
        { 
            var user = await _userService.LoadUserDetail(id);

            return user == null
                       ? (IActionResult)NotFound()
                       : View(user);
        }

        public IActionResult AddUser() => 
          View();

        [HttpPost]
        public async Task<IActionResult> AddUser(CreateUserRequest model, [FromServices] IValidator<CreateUserRequest> validator)
        {
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
            return RedirectToAction("Index", "User");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteUser([FromRoute] string id)
        {
            var response = await _userService.DeleteUser(id);
            _notyf.Success("User Deleted Succesfully"); 
            return RedirectToAction("Index", "User");
        }
         
        public async Task<IActionResult> EditUser(string id) 
        {
            var response = await _userService.LoadUserDetail(id);
            if (response == null)
            {
                return NotFound();
            }
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser([FromRoute] string id, [FromForm] UserDto model, [FromServices] IValidator<UpdateUserRequest> validator)
        {
            //var validationResult = await validator.ValidateAsync(model);
            //if (!validationResult.IsValid)
            //{
            //    foreach (var error in validationResult.Errors)
            //    {
            //        ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            //    }
            //    return View(model);
            //}
            var response = await _userService.UpdateUser(id, model);
            _notyf.Success("User Updated Succesfully");
            return RedirectToAction("Index", "Category");
        }
    }
}
