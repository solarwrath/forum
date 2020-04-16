using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FORUM_PROJECT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FORUM_PROJECT.Controllers
{
    public class TopicController : Controller
    {
        private ILogger<TopicController> _logger;
        private ForumContext _forumContext;
        public TopicController(ILogger<TopicController> logger, ForumContext forumContext)
        {
            _logger = logger;
            _forumContext = forumContext;
        }
        public async Task<IActionResult> Index(int topicId)
        {
            Topic topic = await _forumContext.Topics.FindAsync(topicId);
            _forumContext.Entry(topic).Collection(topic => topic.Posts).Load();
            topic.Posts.ToList().ForEach(post => _forumContext.Entry(post).Reference(post => post.Author).Load());

            _logger.LogCritical($"Got topic id: {topicId}. Topic is null : {topic == null}; PostsCount: {topic.Posts.Count()}");
            //TODO Handle error

            return View(topic);
        }
    }
}