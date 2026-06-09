using Bulletingboard.DTO.Comment;
using Bulletingboard.Entity;
using Bulletingboard.Requests.Comment;
using Bulletingboard.Services.Comment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bulletingboard.Controllers
{
    public class CommentController:Controller
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        /// <summary>
        /// Post-New comment
        /// POST: /Comment/Create
        /// </summary>
        /// <param name="commentRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CommentRequest commentRequest)
        {
            var newComment = new CommentDto(commentRequest);

            int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId);
            var username = User.FindFirstValue(ClaimTypes.Name);
            newComment.UserId = userId;
            newComment.UserName = username;
            await _commentService.AddCommentAsync(newComment);

            return RedirectToAction("Index", "Post");
        }

        /// <summary>
        /// Delete-A comment
        /// Delete: /Comment/:id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _commentService.DeleteCommentAsync(id);
            return RedirectToAction("Index", "Post");
        }
    }
}
