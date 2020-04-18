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
        private readonly PostService _postService;
        public PostController(
            PostService postService
        )
        {
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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddPost([FromBody] AddPostViewModel viewModel)
        {
            bool success = await _postService.AddPostAsync(viewModel.TopicId, viewModel.Message);

            return Json(new
            {
                success,
            });
        }
    }
}