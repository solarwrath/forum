using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FORUM_PROJECT.Models
{
    public class User
    {
        public int Id { get; set; }
        [MaxLength(20)]
        public string Username { get; set; }
        [MaxLength(30)]
        [MinLength(6)]
        public string Password { get; set; }
        [EmailAddress]
        [MaxLength(254)]
        public string Email { get; set; }
    }
}
