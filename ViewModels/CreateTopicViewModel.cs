using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FORUM_PROJECT.ViewModels
{
    public class CreateTopicViewModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Message { get; set; }
    }
}
