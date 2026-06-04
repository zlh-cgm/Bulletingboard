using Bulletingboard.DTO.Comment;
using CommentEntity = Bulletingboard.Entity.Comment;

namespace Bulletingboard.DAO.Comment
{
    public interface ICommentDao
    {
        Task<List<CommentDto>> DbGetCommentByPostIdAsync(int postId);
        Task<CommentEntity?> DbGetCommentByIdAsync(int commentId);
        Task DbAddCommentAsync(CommentEntity Comment);
        Task DbDeleteCommentAsync(int commentId);
    }
}
