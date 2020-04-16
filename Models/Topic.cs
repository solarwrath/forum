using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FORUM_PROJECT.Models
{
    public class Topic
    {
        public string Title { get; set; }
        public IEnumerable<Post> Posts { get; set; }

        public uint ViewCounter { get; set; }
    }
}
