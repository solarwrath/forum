using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FORUM_PROJECT.DAL;
using FORUM_PROJECT.Models;
using FORUM_PROJECT.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FORUM_PROJECT.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly UserService _userService;

        public AuthController(
            ILogger<AuthController> logger,
            UserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToActionPermanent("Index", "TopicList");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(string username, string password)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToActionPermanent("Index", "TopicList");
            }

            bool loginSuccessful = await _userService.TryLoginUserAsync(username, password);

            if (loginSuccessful)
            {
                return RedirectToActionPermanent("Index", "TopicList");
            }

            ViewData["hasLoginError"] = true;

            return View();
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToActionPermanent("Index", "TopicList");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(UserSignUpViewModel viewModel)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToActionPermanent("Index", "TopicList");
            }

            if (!TryValidateModel(viewModel, nameof(UserSignUpViewModel)))
            {
                _logger.LogError($"Validation errors for sign up of user with username '{viewModel.Username}': " +
                                 $"{ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage)}");
                return View();
            }

            string username = viewModel.Username;
            string email = viewModel.Email;
            string password = viewModel.Password;

            IEnumerable<String>? errors = await _userService.TrySignUpUserAsync(username, password, email);

            if (errors == null)
            {
                return RedirectToActionPermanent("SentConfirmationEmail");
            }

            ViewData["signUpErrors"] = errors;
            return View();
        }

        [HttpGet]
        public IActionResult SentConfirmationEmail()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            bool confirmedSuccessfully = await _userService.TryConfirmEmail(userId, code);

            if (confirmedSuccessfully)
            {
                return View("ConfirmationEmailSuccess");
            }

            return View("ConfirmationEmailError");
        }

        public async Task<IActionResult> LogOut()
        {
            await _userService.SignOutUserAsync();
            return RedirectToActionPermanent("LogIn", "Auth");
        }
    }
}