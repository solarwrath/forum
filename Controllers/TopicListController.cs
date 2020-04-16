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
    public class TopicListController : Controller
    {
        private ILogger<TopicListController> _logger;
        private ForumContext _forumContext;

        public TopicListController(ILogger<TopicListController> logger, ForumContext forumContext)
        {
            _logger = logger;
            _forumContext = forumContext;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogCritical("In topiclist");
            IEnumerable<Topic> topics = await _forumContext.Topics.ToListAsync();

            return View(topics);
        }
    }
}