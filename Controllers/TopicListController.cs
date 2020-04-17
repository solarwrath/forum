using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FORUM_PROJECT.DAL;
using FORUM_PROJECT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FORUM_PROJECT.Controllers
{
    public class TopicListController : Controller
    {
        private ILogger<TopicListController> _logger;
        private TopicService _topicService;

        public TopicListController(ILogger<TopicListController> logger, TopicService topicService)
        {
            _logger = logger;
            _topicService = topicService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Topic> topics = await _topicService.GetAllTopicsAsync();

            return View(topics);
        }
    }
}