using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FORUM_PROJECT.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToActionPermanent("Index", "TopicList");
        }
    }
}
