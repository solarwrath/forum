using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FORUM_PROJECT.Models
{
    public class ForumDataSeeder
    {
        public static void SeedForumData(ForumContext context)
        {
            //Drop and recreate database
            context.Database.EnsureDeleted();
            context.Database.Migrate();

            //TODO Get better demo data
            var users = new List<User>
                {
                    new User {Email = "test@gmail.com", Password = "1234567", Username = "testUser1"},
                    new User {Email = "test1@gmail.com", Password = "1234567", Username = "testUser2"},
                    new User {Email = "test2@gmail.com", Password = "1234567", Username = "testUser3"},
                    new User {Email = "test3@gmail.com", Password = "1234567", Username = "testUser4"},
                };

            context.Users.AddRange(users);

            var posts = new List<Post>
                {
                    new Post {Author = users[0], Message = "Post message 1", TimePublished = DateTime.Now},
                    new Post {Author = users[1], Message = "Post message 2", TimePublished = DateTime.Now},
                };

            var topics = new List<Topic>
                {
                    new Topic{Posts = posts, Title="My topic"}
                };
            context.Topics.AddRange(topics);

            context.Posts.AddRange(posts);

            context.SaveChanges();
        }
    }
}