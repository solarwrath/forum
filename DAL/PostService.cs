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
        private ILogger<PostService> _logger;
        private IGenericRepository<Post> _repository;
        private UserManager<User> _userManager;
        private IHttpContextAccessor _httpContextAccessor;
        private ForumContext _forumContext;

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
            Post post = await _repository.GetAsync(post => post.Id == postId);

            if (post == null)
            {
                _logger.LogError($"There is no post with id: {postId}");

                return false;
            }
            
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

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
    }
}
