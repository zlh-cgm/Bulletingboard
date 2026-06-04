using Bulletingboard.DTO.Post;
using Bulletingboard.Entity;
using Bulletingboard.Requests.Post;
using Bulletingboard.Services.Comment;
using Bulletingboard.Services.Post;
using Bulletingboard.ViewModels.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

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


        // GET: /Post
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int.TryParse(userIdString, out int userId);
         
            var posts = await _postService.GetPublicPostListAsync(userId);
      
            return View(new PostListViewModel() { Data=posts});
        }

        // GET: /Post/Details/5
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

        // GET: /Post/Create
        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Post/Create
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

            postRequest.Id = userId;
            var post = new PostDto() {
            Description = postRequest.Description,
            IsPrivate = postRequest.IsPrivate,
            UserId = userId,
            };
            await _postService.AddPostAsync(post);
            return RedirectToAction(nameof(Index));
        }

        // ==========================================
        // GET: /Post/Edit/5
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
            Console.WriteLine($"userId:{userId}, postUserId:{post.UserId}");
            if (userId != post.UserId)
            { 
                return Forbid();
            }
            return View(new PostRequest(post));
        }

        // POST: /Post/Edit/5
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

        // ==========================================
        // GET: /Post/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
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

            return View(post);
        }

        // POST: /Post/Delete/5
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
