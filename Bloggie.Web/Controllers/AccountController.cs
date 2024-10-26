using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IDatabaseLogger _logger;

        public AccountController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager, IDatabaseLogger logger)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _logger = logger;
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var identityUser = new IdentityUser
                {
                    UserName = registerViewModel.Username,
                    Email = registerViewModel.Email
                };

                var identityResult = await userManager.CreateAsync(identityUser, registerViewModel.Password);

                if (identityResult.Succeeded)
                {
                    // assign this user the "User" role
                    var roleIdentityResult = await userManager.AddToRoleAsync(identityUser, "User");

                    if (roleIdentityResult.Succeeded)
                    {
                        // Show success notification
                        return RedirectToAction("Register");
                    }
                }
            }

            // Show error notification
            return View();
        }


        [HttpGet]
        public IActionResult Login(string ReturnUrl)
        {
            var model = new LoginViewModel
            {
                ReturnUrl = ReturnUrl
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(loginViewModel);
                }

                var signInResult = await signInManager.PasswordSignInAsync(
                    loginViewModel.Username,
                    loginViewModel.Password,
                    false,
                    false
                );

                if (signInResult.Succeeded)
                {
                    await _logger.LogAsync($"User '{loginViewModel.Username}' logged in successfully.", "Info");
                    if (!string.IsNullOrWhiteSpace(loginViewModel.ReturnUrl))
                    {
                        return Redirect(loginViewModel.ReturnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }

                await _logger.LogAsync($"Invalid login attempt for username: '{loginViewModel.Username}'.", "Warning");
                ModelState.AddModelError(string.Empty, "Invalid username or password. Please try again.");
                return View(loginViewModel);
            }
            catch (Exception ex)
            {
                await _logger.LogAsync("An error occurred during login.", "Error", ex.ToString());
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
                return View(loginViewModel);
            }
        }



        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}