using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FORUM_PROJECT.Models
{
    public class Post
    {
        public int Id { get; set; }
        //Optional cause we can preserve post even if it's author has been deleted
        public virtual IdentityUser? Author { get; set; }
        public string Message { get; set; }
        public virtual int TopicId { get; set; }
        public virtual Topic Topic { get; set; }
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
