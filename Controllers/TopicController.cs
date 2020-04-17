using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FORUM_PROJECT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FORUM_PROJECT.Controllers
{
    public class TopicController : Controller
    {
        private ILogger<TopicController> _logger;
        private ForumContext _forumContext;
        private UserManager<User> _userManager;

        public TopicController(
            ILogger<TopicController> logger,
            ForumContext forumContext,
            UserManager<User> userManager)
        {
            _logger = logger;
            _forumContext = forumContext;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Index(int topicId)
        {
            var topic = await _forumContext.Topics.FindAsync(topicId);
            _forumContext.Entry(topic).Collection(topic => topic.Posts).Load();
            topic.Posts.ToList().ForEach(post => _forumContext.Entry(post).Reference(post => post.Author).Load());

            _logger.LogCritical($"Got topic to display id: {topicId}. Topic is null : {topic == null}; PostsCount: {topic.Posts.Count()}");
            //TODO Handle error

            return View(topic);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddPost(string postMessage, int topicId)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var topic = await _forumContext.Topics.FindAsync(topicId);

            Post newPost = new Post {Author = user, Message = postMessage, TimePublished = DateTime.Now, Topic = topic, TopicId = topicId };

            await _forumContext.AddAsync(newPost);
            await _forumContext.SaveChangesAsync();

            return RedirectToActionPermanent("Index", new {topicId=topicId});
        }
    }
}