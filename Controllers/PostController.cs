using Bulletingboard.DTO.Post;
using Bulletingboard.Requests.Post;
using Bulletingboard.Services.Post;
using Bulletingboard.ViewModels.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bulletingboard.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        private readonly ILogger<PostController> _logger;

        public PostController(IPostService postService,ILogger<PostController> logger)
        {
            _postService = postService;
            _logger = logger;
        }


        /// <summary>
        /// Get-All Public Posts and private Posts of logined user(if not logined, no private post)
        /// GET: /Post
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int.TryParse(userIdString, out int userId);
         
            var posts = await _postService.GetPublicPostListAsync(userId);
      
            return View(new PostListViewModel() { Data=posts});
        }

        /// <summary>
        /// Get-Detail view of Post
        /// GET: /Post/:id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _postService.GetPostByIdAsync(id.Value);
            if (post == null)
            {
                return NotFound();
            }
            
            int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId);
            if (post.IsPrivate && post.UserId != userId)
            {
                return Forbid();
            }

            return View(new PostDetailViewModel() { Data=post});
        }

        /// <summary>
        /// Get-View of create Post
        /// GET: /Post/Create
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Post-New create Post
        /// POST: /Post/Create
        /// </summary>
        /// <param name="postRequest"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostRequest postRequest)
        {
            if (!ModelState.IsValid)
            {
                return View(postRequest);
            }

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int.TryParse(userIdString, out int userId);

            var post = new PostDto() {
            Description = postRequest.Description,
            IsPrivate = postRequest.IsPrivate,
            UserId = userId,
            };
            await _postService.AddPostAsync(post);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Get-View of edit Post
        /// GET: /Post/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _postService.GetPostByIdAsync(id.Value);
            if (post == null)
            {
                return NotFound();
            }
            int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId);

            if (userId != post.UserId)
            { 
                return Forbid();
            }
            return View(new PostRequest(post));
        }

        /// <summary>
        /// Post-Edit a Post
        /// POST: /Post/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="postRequest"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PostRequest postRequest)
        {
            if (id != postRequest.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(postRequest);
            }

            await _postService.UpdatePostAsync(new PostDto(postRequest));
            return RedirectToAction(nameof(Index));
        }


        /// <summary>
        /// Post-Delete a Post
        /// POST: /Post/Delete/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _postService.GetPostByIdAsync(id.Value);
            int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId);
            if (userId != post.UserId)
            {
                return Forbid();
            }
            await _postService.DeletePostAsync(id.Value);
            return RedirectToAction(nameof(Index));
        }
    }
}
