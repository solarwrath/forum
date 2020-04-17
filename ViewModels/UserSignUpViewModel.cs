using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FORUM_PROJECT.ViewModels
{
    public class UserSignUpViewModel
    {
        //TODO Change these validations
        [Required]
        [StringLength(60, MinimumLength = 3)]
        public string Username { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [StringLength(60, MinimumLength = 4)]
        public string Password { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }
    }
}
