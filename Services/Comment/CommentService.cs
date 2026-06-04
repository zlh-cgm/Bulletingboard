using Bulletingboard.DTO.Comment;
using Bulletingboard.DAO.Comment;

namespace Bulletingboard.Services.Comment
{
    public class CommentService:ICommentService
    {
        private readonly ICommentDao _commentDao;

        public CommentService(ICommentDao commentDao)
        { 
            _commentDao = commentDao;
        }

        public async Task<List<CommentDto>> GetCommentByPostIdAsync(int postId)
        {
            return await _commentDao.DbGetCommentByPostIdAsync(postId);
        }
        public async Task AddCommentAsync(CommentDto commentDto)
        { 
            var newComment=commentDto.BindDbModel();
            await _commentDao.DbAddCommentAsync(newComment);
        }

        public async Task DeleteCommentAsync(int commentId)
        { 
            await _commentDao.DbDeleteCommentAsync(commentId);
        }
    }
}
