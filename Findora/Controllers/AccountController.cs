using Findora.Data;
using Findora.Models;
using Findora.Services.Interfaces;
using Findora.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Findora.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILocationService _ilocationService;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILocationService iLocationService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _ilocationService = iLocationService;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl)
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");
            ViewData["ReturnUrl"] = returnUrl;
            return View("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveLogin(LoginViewModel modelFromRequest, string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                //check found
                ApplicationUser appUser = await _userManager.FindByEmailAsync(modelFromRequest.Email);
                if(appUser != null)
                {
                    var found = await _signInManager.PasswordSignInAsync(
                        appUser,
                        modelFromRequest.Password,
                        modelFromRequest.RememberMe,
                        lockoutOnFailure: true);
                    if (found.Succeeded)
                    {
                        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                            return LocalRedirect(returnUrl);
                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError("", "Wrong Email Or Password");
            }
            ViewData["ReturnUrl"] = returnUrl;
            return View("Login", modelFromRequest);
        }


        [HttpGet]
        public async Task<IActionResult> GetCities(int governorateId)
        {
            if (governorateId <= 0)
                return BadRequest();

            var cities = await _ilocationService
                .GetCitiesByGovernorateIdAsync(governorateId);

            var result = cities.Select(c => new
            {
                id = c.Id,
                name = c.Name
            });

            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");
            var model = new RegisterViewModel
            {
                Governorates = await _ilocationService.GetAllGovernoratesAsync()
            };

            return View("Register", model);
        }

        // for validation error
        private async Task LoadRegisterData(RegisterViewModel model)
        {
            model.Governorates =
                await _ilocationService.GetAllGovernoratesAsync();

            if (model.GovernorateId > 0)
            {
                model.Cities =
                    await _ilocationService.GetCitiesByGovernorateIdAsync(
                        model.GovernorateId);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddStudent(RegisterViewModel modelFromRequest)
        {
            if (modelFromRequest.GovernorateId <= 0)
            {
                ModelState.AddModelError(
                    nameof(modelFromRequest.GovernorateId), "Please select a governorate.");
            }

            if (modelFromRequest.CityId <= 0)
            {
                ModelState.AddModelError(
                    nameof(modelFromRequest.CityId), "Please select a city.");
            }
            
            if(modelFromRequest.CityId > 0 && modelFromRequest.GovernorateId > 0)
            {
                var isValid = await _ilocationService.CityBelongsToGovernorateAsync(
                    modelFromRequest.CityId,
                    modelFromRequest.GovernorateId);

                if (!isValid)
                    ModelState.AddModelError(nameof(modelFromRequest.CityId),
                        "The selected city does not belong to this governorate.");
            }

            if (ModelState.IsValid)
            {
                //mapping
                var appUser = new ApplicationUser
                {
                    UserName = modelFromRequest.Email,
                    Email = modelFromRequest.Email,
                    FullName = modelFromRequest.FullName,
                    GovernorateId = modelFromRequest.GovernorateId,
                    CityId = modelFromRequest.CityId
                };

                var result = await _userManager.CreateAsync(appUser, modelFromRequest.Password);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors.Where(e => e.Code != "DuplicateUserName"))
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                else
                {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, AppRoles.Student);
                    if (!roleResult.Succeeded)
                    {
                        await _userManager.DeleteAsync(appUser);

                        foreach (var error in roleResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                    else
                    {
                        TempData["Success"] = "Account created. You can log in now.";
                        return RedirectToAction("Login");
                    }
                }
            }
            await LoadRegisterData(modelFromRequest);
            return View("Register", modelFromRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult AccessDenied() => View();
    }

}

