using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using FORUM_PROJECT.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace FORUM_PROJECT.DAL
{
    public class PostService
    {
        private readonly ILogger<PostService> _logger;
        private readonly IGenericRepository<Post> _repository;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ForumContext _forumContext;

        public PostService(
            ILogger<PostService> logger,
            IGenericRepository<Post> repository,
            UserManager<User> userManager,
            IHttpContextAccessor httpContextAccessor,
            ForumContext forumContext)
        {
            _logger = logger;
            _repository = repository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _forumContext = forumContext;
        }

        public async Task<bool> TryEditPostAsync(int postId, string newMessage)
        {
            _logger.LogInformation($"Attempting to edit post with id {postId}, new message: {newMessage}");

            if (newMessage.Length == 0)
            {
                _logger.LogError($"Editing post failed, message is empty!");
                return false;
            }

            Post? post = await _repository.GetAsync(post => post.Id == postId);

            if (post == null)
            {
                _logger.LogError($"There is no post with id: {postId}");

                return false;
            }

            User? user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            if (user == null)
            {
                _logger.LogError("Invalid current user: user is null");

                return false;
            }

            if (user != post.Author)
            {
                _logger.LogError($"Invalid current user: {user}. Post author: {post.Author}");

                return false;
            }

            post.Message = newMessage;
            await _forumContext.SaveChangesAsync();

            _logger.LogInformation($"Edited post with id {postId}, new message: {newMessage}");

            return true;
        }

        public async Task<bool> AddPostAsync(int topicId, string postMessage)
        {
            _logger.LogInformation($"Adding new post{{topicId: {topicId}; postMessage: {postMessage}}}");

            if (postMessage.Length == 0)
            {
                _logger.LogError($"Adding new post failed, message is empty!");
                return false;
            }

            User? user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if (user == null)
            {
                _logger.LogError($"Adding new post failed, current user is null!");
                return false;
            }

            Topic? topic = await _forumContext.Topics.FindAsync(topicId);
            if (topic == null)
            {
                _logger.LogError($"Adding new post failed, there is no such topic!");
                return false;
            }

            Post newPost = new Post { Author = user, Message = postMessage, TimePublished = DateTime.Now, Topic = topic, TopicId = topicId };

            await _forumContext.AddAsync(newPost);
            await _forumContext.SaveChangesAsync();

            _logger.LogInformation($"Have successfully added new post: {newPost}");

            return true;
        }
    }
}
