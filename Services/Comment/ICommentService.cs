using Bulletingboard.DTO.Comment;

namespace Bulletingboard.Services.Comment
{
    public interface ICommentService
    {
        /// <summary>
        /// Retrieves all comments for a specific post
        /// </summary>
        /// <param name="postId">postId</param>
        /// <returns></returns>
        Task<List<CommentDto>> GetCommentByPostIdAsync(int postId);
        /// <summary>
        /// Creates and saves a new comment
        /// </summary>
        /// <param name="commentDto">Id,Content,PostId,UserId,UserName,CreatedAt</param>
        /// <returns></returns>
        Task AddCommentAsync(CommentDto commentDto);
        /// <summary>
        /// Deletes a specific comment by its ID
        /// </summary>
        /// <param name="commentId">commentId</param>
        /// <returns></returns>
        Task DeleteCommentAsync(int commentId);
    }
}
