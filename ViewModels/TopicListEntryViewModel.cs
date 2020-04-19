using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FORUM_PROJECT.Models
{
    public class TopicListEntryViewModel
    {
        public int TopicId { get; set; }
        public string Title { get; set; }
        public uint Views { get; set; }
        public string AuthorUsername { get; set; }
        public uint Replies { get; set; }
        public string LastActivity { get; set; }
    }
}