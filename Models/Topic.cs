using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FORUM_PROJECT.Models
{
    public class Topic
    {
        public int Id { get; set; }

        [MaxLength(120)]
        public string Title { get; set; }
        public IEnumerable<Post> Posts { get; set; }
        public uint ViewCounter { get; set; } = 0;
    }
}
