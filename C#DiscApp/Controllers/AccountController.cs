using C_DiscApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using C_DiscApp.Views.ViewModels;

namespace C_DiscApp.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<User> userManager;
        private SignInManager<User> signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        // Register
        [HttpGet("Account/Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("Account/Register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User { UserName = model.Username };
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }

        // Login
        [HttpGet("Account/LogIn")]
        public IActionResult LogIn(string returnURL = "")
        {
            var model = new LoginViewModel { ReturnUrl = returnURL };
            return View(model);
        }

        [HttpPost("Account/LogIn")]
        public async Task<IActionResult> LogIn(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(
                    model.Username, model.Password,
                    isPersistent: model.RememberMe,
                    lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) &&
                        Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            ModelState.AddModelError("", "Invalid username/password.");
            return View(model);
        }

        // Logout
        [HttpPost("Account/LogOut")]
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // Index
        [HttpGet("Account/Index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
