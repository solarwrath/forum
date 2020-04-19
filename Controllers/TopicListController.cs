using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FORUM_PROJECT.DAL;
using FORUM_PROJECT.Models;
using FORUM_PROJECT.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FORUM_PROJECT.Controllers
{
    public class TopicListController : Controller
    {
        private readonly ILogger<TopicListController> _logger;
        private readonly TopicService _topicService;

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

            var viewModels = topics.Select(topic =>
            {
                TopicListEntryViewModel viewModel = new TopicListEntryViewModel();

                viewModel.TopicId = topic.Id;

                viewModel.Title = topic.Title;
                if (topic.Title.Length > 80)
                {
                    viewModel.Title = topic.Title.Substring(0, Math.Min(topic.Title.Length, 79)) + '…';
                }

                viewModel.Views = topic.ViewCounter;

                var posts = topic.Posts.ToList();
                posts.Sort((a, b) => a.TimePublished.CompareTo(b.TimePublished));

                Post firstPost = posts.First();

                string authorUsername = "UFO";
                if (firstPost.Author != null)
                {
                    authorUsername = firstPost.Author.UserName;

                    if (authorUsername.Length > 20)
                    {
                        authorUsername = authorUsername.Substring(0, Math.Min(authorUsername.Length, 19)) + '…';
                    }
                }

                viewModel.AuthorUsername = authorUsername;

                viewModel.Replies = (uint)(topic.Posts.Count() - 1);

                viewModel.LastActivity = posts.Last().TimePublished.GetElapsedTimeHumanReadable();

                return viewModel;
            });

            _logger.LogInformation("Generated viewmodels for topic list");

            return View(viewModels);
        }
    }
}