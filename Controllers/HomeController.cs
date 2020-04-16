using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FORUM_PROJECT.Models;
using Microsoft.EntityFrameworkCore;

namespace FORUM_PROJECT.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            //TODO Authorization check
            _logger.LogCritical("Redirecting");
            return RedirectToActionPermanent("Index", "TopicList");
        }
    }
}
