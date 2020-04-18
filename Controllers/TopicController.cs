using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FORUM_PROJECT.DAL;
using FORUM_PROJECT.Models;
using FORUM_PROJECT.ViewModels;
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
        private readonly ILogger<TopicController> _logger;
        private readonly ForumContext _forumContext;
        private readonly TopicService _topicService;
        private readonly PostService _postService;

        public TopicController(
            ILogger<TopicController> logger,
            ForumContext forumContext,
            TopicService topicService,
            PostService postService)
        {
            _logger = logger;
            _forumContext = forumContext;
            _topicService = topicService;
            _postService = postService;
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

                await _topicService.incrementViewCounter(topic);

                return View(topic);
            }
            else
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddPost([FromBody] AddPostViewModel viewModel)
        {
            _logger.LogInformation($"Id:{viewModel.TopicId}; Message: {viewModel.Message}");
            bool success = await _postService.AddPostAsync(viewModel.TopicId, viewModel.Message);
            
            return Json(new
            {
                success,
            });
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