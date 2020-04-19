using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using FORUM_PROJECT.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;

namespace FORUM_PROJECT.DAL
{
    public class UserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly LinkGenerator _linkGenerator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(
            ILogger<UserService> logger,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            LinkGenerator linkGenerator,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _linkGenerator = linkGenerator;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> TryLoginUserAsync(string username, string password)
        {
            User? user = await _userManager.FindByNameAsync(username);

            if (user != null)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);

                if (signInResult.Succeeded)
                {
                    _logger.LogInformation($"Successfully signed in the user with username '{username}' ");
                    return true;
                }

                //We do not log passwords as that is not secure
                _logger.LogError($"Unsuccessful login for user with username '{username}'");
            }
            else
            {
                //We do not output error to this case as that is not secure
                _logger.LogError($"Couldn't find user with username '{username}' for login");
            }

            return false;
        }


        //See https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.identityerrordescriber?view=aspnetcore-3.1
        //IdentityError.Code is a nameof that method
        private static readonly string[] SAFE_FOR_OUTPUT_SIGN_UP_ERRORS = new[]
        {
            nameof(IdentityErrorDescriber.LoginAlreadyAssociated),
            nameof(IdentityErrorDescriber.DuplicateEmail),
            nameof(IdentityErrorDescriber.InvalidUserName),
            nameof(IdentityErrorDescriber.InvalidEmail),
            nameof(IdentityErrorDescriber.PasswordMismatch),
            nameof(IdentityErrorDescriber.PasswordRequiresDigit),
            nameof(IdentityErrorDescriber.PasswordRequiresLower),
            nameof(IdentityErrorDescriber.PasswordRequiresNonAlphanumeric),
            nameof(IdentityErrorDescriber.PasswordRequiresUniqueChars),
            nameof(IdentityErrorDescriber.PasswordRequiresUpper),
            nameof(IdentityErrorDescriber.PasswordTooShort)
        };

        public async Task<IEnumerable<string>?> TrySignUpUserAsync(string username, string password, string email)
        {
            username = username.Trim();

            var user = new User
            {
                Email = email,
                UserName = username,
            };
            

            var signUpResult = await _userManager.CreateAsync(user, password);

            if (signUpResult.Succeeded)
            {
                _logger.LogInformation($"Signed up user with username '{username}'");

                await SendConfirmationEmail(user);

                return null;
            }

            _logger.LogError($"Couldn't register user with username '{username}'", signUpResult.Errors);


            //Output Only Safe errors
            var errors = signUpResult.Errors.ToList()
                .Where(error => SAFE_FOR_OUTPUT_SIGN_UP_ERRORS.Contains(error.Code));

            if (errors.Count() != 0)
            {
                return errors.Select(error => error.Description); ;
            }

            return new[] { "Couldn't sign up user!" };
        }

        public async Task SignOutUserAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<bool> SendConfirmationEmail(User user)
        {
            string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            
            var url = _linkGenerator.GetUriByAction(_httpContextAccessor.HttpContext,
                "ConfirmEmail", "Auth", new
                {
                    userId = user.Id,
                    code
                });
            _logger.LogInformation($"Generated confirmation link for user '{user.UserName}': {url}");

            string message = $"Hi there, {user.UserName}.\n" +
                             $"We are glad that you have decided to participate in the discussions!\n" +
                             $"Visit <a href=\"{url}\">this link</a> to finish registration,\n" +
                             $"Forum";

            string emailFromAddress = Environment.GetEnvironmentVariable("EMAIL_FROM_ADDRESS");
            string smtpPassword = Environment.GetEnvironmentVariable("SMTP_PASSWORD");
            string smtpAddress = Environment.GetEnvironmentVariable("SMTP_ADDRESS");
            bool portNumberParsedSuccess = int.TryParse(Environment.GetEnvironmentVariable("SMTP_PORT_NUMBER"), out int portNumber);

            if (!portNumberParsedSuccess)
            {
                _logger.LogError($"Couldn't parse smtp port number: {Environment.GetEnvironmentVariable("SMTP_PORT_NUMBER")}");
                return false;
            }

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(emailFromAddress);
                mail.To.Add(user.Email);
                mail.Subject = "Forum Sign Up Confirmation";
                mail.Body = message;
                mail.IsBodyHtml = true;
                using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                {
                    smtp.Credentials = new NetworkCredential(emailFromAddress, smtpPassword);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }

            _logger.LogInformation($"Sent confirmation link for user '{user.UserName}' with mail ${user.Email}");

            return true;
        }

        public async Task<bool> TryConfirmEmail(string userId, string code)
        {
            User? user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return false;
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);

            return result.Succeeded;
        }
    }
}