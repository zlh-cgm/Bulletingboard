using Bulletingboard.DTO.Comment;
using CommentEntity = Bulletingboard.Entity.Comment;

namespace Bulletingboard.DAO.Comment
{
    public interface ICommentDao
    {
        /// <summary>
        /// Retrieves all comments for a specific post from the database
        /// </summary>
        /// <param name="postId">postId</param>
        /// <returns></returns>
        Task<List<CommentDto>> DbGetCommentByPostIdAsync(int postId);
        /// <summary>
        /// Retrieves a single comment by its ID from the database
        /// </summary>
        /// <param name="commentId">commentId</param>
        /// <returns></returns>
        Task<CommentEntity?> DbGetCommentByIdAsync(int commentId);
        /// <summary>
        /// Inserts a new comment into the database
        /// </summary>
        /// <param name="Comment">Id,Content,PostId,UserId,UserName,CreatedAt</param>
        /// <returns></returns>
        Task DbAddCommentAsync(CommentEntity Comment);
        /// <summary>
        /// Removes a specific comment from the database by its ID
        /// </summary>
        /// <param name="commentId">commentId</param>
        /// <returns></returns>
        Task DbDeleteCommentAsync(int commentId);
    }
}
