using Bulletingboard.DTO.Comment;

namespace Bulletingboard.Services.Comment
{
    public interface ICommentService
    {
        Task<List<CommentDto>> GetCommentByPostIdAsync(int postId);
        Task AddCommentAsync(CommentDto commentDto);

        Task DeleteCommentAsync(int commentId);
    }
}
