using BlogEngine.Core.Contracts;
using BlogEngine.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BlogEngine.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly ILogger<BlogController> _logger;
        private readonly IBlogService _blogService;

        public BlogController(ILogger<BlogController> logger, IBlogService blogService)
        {
            _blogService = blogService;
            _logger = logger;
        }

        /// <summary>
        /// Endpoint to get all the posts by status
        /// </summary>
        /// <param name="status">Status to be searched</param>
        /// <returns>List of posts</returns>
        [HttpGet]
        [Route("post/GetPostsByStatus/{status}")]
        public async Task<IActionResult> GetPostsByStatus(Status status)
        {
            try
            {
                var posts = await _blogService.GetPostsByStatus(status);
                if (!posts.Any()) return NotFound();
                return Ok(posts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "BlogController::GetPostsByStatus");
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        /// <summary>
        ///  Endpoint to get all the posts by writer id
        /// </summary>
        /// <param name="writerId">Writer id to be searched</param>
        /// <returns>List of posts</returns>
        [HttpGet]
        [Route("post/GetPostsByWriter/{writerId}")]
        public async Task<IActionResult> GetPostsByWriter(string writerId)
        {
            try
            {
                var posts = await _blogService.GetPostsByWriter(writerId);
                if (!posts.Any()) return NotFound();
                return Ok(posts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "BlogController::GetPostsByWriter");
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        /// <summary>
        /// Endpoint to create or update a post
        /// </summary>
        /// <param name="post">Post to be created or updated</param>
        /// <returns>true if it was succeeded otherwise false</returns>
        [HttpPut, HttpPost]
        [Route("post/CreateUpdatePost")]
        public IActionResult CreateUpdatePost(Post post)
        {
            try
            {
                string approverId = HttpContext.Request.Headers["UserId"].ToString();
                var processed = _blogService.CreateUpdatePost(post, approverId);

                return Ok(processed);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "BlogController::CreateUpdatePost");
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        /// <summary>
        /// Endpoint to update the status of a post
        /// </summary>
        /// <param name="postId">postId to be updated</param>
        /// <param name="status">status to be setted</param>
        /// <returns>true if it was succeeded otherwise false</returns>
        [HttpPut, HttpPost]
        [Route("post/UpdateStatePost/{postId}/{status}")]
        public IActionResult UpdateStatePost(int postId, Status status)
        {
            try
            {
                string approverId = HttpContext.Request.Headers["UserId"].ToString();

                var updated = _blogService.UpdateStatePost(postId, status, approverId);

                return Ok(updated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "BlogController::UpdateStatePost");
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        /// <summary>
        /// Endpoint to create a comment
        /// </summary>
        /// <param name="comment">Comment to be created</param>
        /// <returns>true if it was succeeded otherwise false</returns>
        [HttpPost]
        [Route("comment/CreateComment")]
        public IActionResult CreateComment(Comment comment)
        {
            try
            {
                var created = _blogService.CreateComment(comment);

                return Ok(created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "BlogController::CreateComment");
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

    }
}
