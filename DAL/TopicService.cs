using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FORUM_PROJECT.Models;
using Microsoft.Extensions.Logging;

namespace FORUM_PROJECT.DAL
{
    public class TopicService
    {
        private ILogger<TopicService> _logger;
        private IGenericRepository<Topic> _repository;

        public TopicService(ILogger<TopicService> logger, IGenericRepository<Topic> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public IEnumerable<Topic> GetAllTopics()
        {
            IEnumerable<Topic>  topics = _repository.GetAll();
            _logger.LogDebug($"Got topics: {topics}");

            return topics;
        }

        public async Task<IEnumerable<Topic>> GetAllTopicsAsync()
        {
            return await _repository.GetAllAsync();
        }
    }
}
