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
        private ILogger<TopicController> _logger;
        private ForumContext _forumContext;
        private UserManager<User> _userManager;
        public TopicService _topicService;

        public TopicController(
            ILogger<TopicController> logger,
            ForumContext forumContext,
            UserManager<User> userManager,
            TopicService topicService)
        {
            _logger = logger;
            _forumContext = forumContext;
            _userManager = userManager;
            _topicService = topicService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index(int topicId)
        {
            //TODO Handle error
            var topic = await _forumContext.Topics.FindAsync(topicId);

            if (topic != null)
            {
                _logger.LogInformation($"Got topic with index: {topicId}");
                await _forumContext.Entry(topic).Collection(topic => topic.Posts).LoadAsync();
                topic.Posts.ToList().ForEach(post =>
                {
                    _forumContext.Entry(post).Reference(post => post.Author).Load();
                    _logger.LogInformation(post.Author.UserName);
                });
                await _topicService.incrementViewCounter(topic);

                return View(topic);
            }
            else
            {
                _logger.LogError($"Tried to load topic with index: {topicId} but there is no such topic!");

                return NotFound();
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddPost(string postMessage, int topicId)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var topic = await _forumContext.Topics.FindAsync(topicId);

            Post newPost = new Post { Author = user, Message = postMessage, TimePublished = DateTime.Now, Topic = topic, TopicId = topicId };

            await _forumContext.AddAsync(newPost);
            await _forumContext.SaveChangesAsync();

            return RedirectToActionPermanent("Index", new { topicId });
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