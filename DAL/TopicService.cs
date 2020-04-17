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
        private GenericRepository<Topic> _repository;

        public TopicService(ILogger<TopicService> logger, GenericRepository<Topic> repository)
        {
            _logger = logger;
            _repository = repository;
        }
    }
}
