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
    public class TopicService
    {
        private readonly ILogger<TopicService> _logger;
        private readonly IGenericRepository<Topic> _repository;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ForumContext _forumContext;

        public TopicService(
            ILogger<TopicService> logger,
            IGenericRepository<Topic> repository,
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

        public async Task<Topic?> GetTopicAsync(int topicId)
        {
            Topic? topic = await _repository.GetAsync(topic => topic.Id == topicId);

            if (topic == null)
            {
                _logger.LogError($"Attempted to find topic with id {topicId}, but there is no such topic!");
            }
            else
            {
                _logger.LogInformation($"Found topic with id {topicId}!");
            }

            return topic;
        }

        public async Task<IEnumerable<Topic>> GetAllTopicsAsync()
        {
            IEnumerable<Topic> topics = await _repository.GetAllAsync();
            topics.ToList().ForEach(topic =>
            {
                _forumContext.Entry(topic).Collection(topic => topic.Posts).Load();

                var posts = topic.Posts.ToList();
                posts.Sort((a, b) => a.TimePublished.CompareTo(b.TimePublished));

                _forumContext.Entry(posts.First()).Reference(post => post.Author).Load();
            });

            _logger.LogDebug($"Got topics: {topics}");

            return topics;
        }

        public async Task<EntityEntry<Topic>> CreateTopic(string title, string message)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            Topic topic = new Topic { Title = title.Trim() };
            topic.Posts = new[] { new Post { Topic = topic, Author = user, Message = message, TimePublished = DateTime.Now } };
            _logger.LogInformation($"Created topic: {topic}");

            return await _repository.AddAsync(topic);
        }

        public async Task incrementViewCounter(Topic topic)
        {
            topic.ViewCounter++;
            await _forumContext.SaveChangesAsync();
            _logger.LogInformation($"Incremented view counter for topic: {topic.Id}. New view counter: {topic.ViewCounter}");
        }
    }
}
