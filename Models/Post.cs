using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FORUM_PROJECT.Models
{
    public class Post
    {
        public int Id { get; set; }
        public User Author { get; set; }
        public string Message { get; set; }
        public int TopicId { get; set; }
        public Topic Topic { get; set; }
        public DateTime TimePublished { get; set; }
    }
}
