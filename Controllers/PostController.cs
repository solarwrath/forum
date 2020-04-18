using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FORUM_PROJECT.DAL;
using FORUM_PROJECT.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FORUM_PROJECT.Controllers
{
    public class PostController : Controller
    {
        private ILogger<PostController> _logger;
        private PostService _postService;
        public PostController(
            PostService postService,
            ILogger<PostController> logger
        )
        {
            _logger = logger;
            _postService = postService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditPost([FromBody] EditPostViewModel editPostViewModel)
        {
            bool success = await _postService.TryEditPostAsync(editPostViewModel.PostId, editPostViewModel.NewMessage);

            return Json(new
            {
                success,
            });
        }
    }
}