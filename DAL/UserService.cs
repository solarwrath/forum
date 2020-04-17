using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FORUM_PROJECT.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;

namespace FORUM_PROJECT.DAL
{
    public class UserService
    {
        private ILogger<UserService> _logger;
        private IGenericRepository<User> _repository;
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;

        public UserService(
            ILogger<UserService> logger,
            IGenericRepository<User> repository,
            UserManager<User> userManager,
            SignInManager<User> signInManager
            )
        {
            _logger = logger;
            _repository = repository;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<bool> TryLoginUserAsync(string username, string password)
        {
            User user = await _userManager.FindByNameAsync(username);

            if (user != null)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);

                if (signInResult.Succeeded)
                {
                    _logger.LogInformation($"Successfully signed in the user with username '{username}' ");
                    return true;
                }
                else
                {
                    //We do not log passwords as that is not secure
                    _logger.LogError($"Unsuccessful login for user with username '{username}'");
                }
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
            var user = new User
            {
                Email = email,
                UserName = username,
            };

            var signUpResult = await _userManager.CreateAsync(user, password);

            if (signUpResult.Succeeded)
            {
                _logger.LogInformation($"Signed up user with username '{username}'");

                //TODO Email verification
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

            return new[] {"Couldn't sign up user!"};
        }

        public async Task SignOutUserAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}