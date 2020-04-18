using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FORUM_PROJECT.Models
{
    public class Post
    {
        public int Id { get; set; }
        //Optional cause we can preserve post even if it's author has been deleted
        public User? Author { get; set; }
        public string Message { get; set; }
        public int TopicId { get; set; }
        public Topic Topic { get; set; }
        public DateTime TimePublished { get; set; }

        public override string ToString()
        {
            string authorUsername = (Author == null) ? "null" : Author.UserName;

            return $"{{Id: {Id};\n" +
                   $"Author: {authorUsername};\n" +
                   $"Message: {Message};\n" +
                   $"TopicId: {TopicId};\n" +
                   $"TimePublished: {TimePublished.ToString()}}}";
        }
    }
}
