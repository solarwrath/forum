using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FORUM_PROJECT.Models
{
    public class ForumDataSeeder
    {
        public static void SeedForumData(ForumContext context,
            UserManager<User> userManager)
        {
            //Drop and recreate database
            context.Database.EnsureDeleted();
            context.Database.Migrate();

            //TODO Get better demo data

            Task.Run(async () =>
            {
                var users = new List<User>
                {
                    new User {Email = "test@gmail.com", UserName = "testUser1"},
                    new User {Email = "test1@gmail.com", UserName = "testUser2"},
                    new User {Email = "test2@gmail.com", UserName = "testUser3"},
                    new User {Email = "test3@gmail.com", UserName = "testUser4"},
                };

                users.ForEach(user =>
                {
                    Task.Run(async ()=> await userManager.CreateAsync(user, "qwerty")).GetAwaiter().GetResult();
                });
            
                var posts = new List<Post>
                {
                    new Post {Author = users[0], Message = "Post message 1", TimePublished = DateTime.Now},
                    new Post {Author = users[1], Message = "Post message 2", TimePublished = DateTime.Now},
                    new Post {Author = users[3], Message = "Post message 3", TimePublished = DateTime.Now},
                };

                var topics = new List<Topic>
                {
                    new Topic {Posts = new List<Post> {posts[0], posts[1]}, Title = "My topic"},
                    new Topic {Posts = new List<Post> {posts[2]}, Title = "My topic"}
                };

                context.Topics.AddRange(topics);

                context.Posts.AddRange(posts);

                context.SaveChanges();
            }).GetAwaiter().GetResult();
        }

        public static async Task RegisterUser(User user, UserManager<User> userManager)
        {
            await userManager.CreateAsync(user, "qwerty");
        }
    }
}
