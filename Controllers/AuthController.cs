using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FORUM_PROJECT.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FORUM_PROJECT.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthController(
            ILogger<AuthController> logger,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToActionPermanent("Index", "TopicList");
            }

            User user = await _userManager.FindByNameAsync(username);

            if (user != null)
            {
                _logger.LogCritical($"Found the user with username: {username} ");
                var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);

                if (signInResult.Succeeded)
                {
                    _logger.LogCritical($"Signed in the user with username: {username} ");
                    return RedirectToActionPermanent("Index", "TopicList");
                }
                else
                {
                    _logger.LogCritical($"User login failed for user: {username} ");
                }
            }
            else
            {
                _logger.LogCritical($"Haven't found the user with username: {username} ");
            }

            return RedirectToActionPermanent("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string password)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToActionPermanent("Index", "TopicList");
            }

            var user = new User
            {
                Email = "tes21t@gmail.com",
                UserName = username,
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                _logger.LogCritical($"Registered user with username: {username}");
            }
            else
            {
                _logger.LogCritical($"Couldn't register user with username: {username}");
            }

            return RedirectToActionPermanent("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToActionPermanent("Index", "Home");
        }
    }
}