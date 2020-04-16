using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FORUM_PROJECT.Models
{
    public class Post
    {
        public User Author { get; set; }
        public string Message { get; set; }

        public DateTime TimePublished { get; set; }
    }
}
