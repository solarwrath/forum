using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FORUM_PROJECT.ViewModels
{
    public class UserSignUpViewModel
    {
        [Required]
        [StringLength(60, MinimumLength = 3)]
        public string Username { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and Confirmation Password must match!")]
        [Display(Name = "Password Confirmation")]
        public string PasswordConfirmation { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }
    }
}
