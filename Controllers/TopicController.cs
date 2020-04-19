using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FORUM_PROJECT.DAL;
using FORUM_PROJECT.Models;
using FORUM_PROJECT.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FORUM_PROJECT.Controllers
{
    public class TopicController : Controller
    {
        private readonly ForumContext _forumContext;
        private readonly TopicService _topicService;

        public TopicController(
            ForumContext forumContext,
            TopicService topicService)
        {
            _forumContext = forumContext;
            _topicService = topicService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index(int topicId)
        {
            Topic? topic = await _topicService.GetTopicAsync(topicId);

            if (topic != null)
            {
                await _forumContext.Entry(topic).Collection(topic => topic.Posts).LoadAsync();
                topic.Posts.ToList().ForEach(post =>
                {
                    _forumContext.Entry(post).Reference(post => post.Author).Load();
                });

                await _topicService.IncrementViewCounter(topic);

                return View(topic);
            }
            else
            {
                return NotFound();
            }
        }
        
        [Authorize]
        [HttpGet]
        public IActionResult CreateTopic()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateTopic(CreateTopicViewModel viewModel)
        {
            var createdTopicEntry = await _topicService.CreateTopic(viewModel.Title, viewModel.Message);

            return RedirectToActionPermanent("Index", new { topicId = createdTopicEntry.Entity.Id });
        }
    }
}