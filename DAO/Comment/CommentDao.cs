using Bulletingboard.DTO.Comment;
using Bulletingboard.Entity;
using Microsoft.EntityFrameworkCore;
using CommentEntity = Bulletingboard.Entity.Comment;

namespace Bulletingboard.DAO.Comment
{
    public class CommentDao:ICommentDao
    {
        private readonly BulletingboardDbContext _context;

        public CommentDao(BulletingboardDbContext context)
        {
            _context = context;
        }
        public async Task<List<CommentDto>> DbGetCommentByPostIdAsync(int postId)
        {
            var comments = await _context.Comments
                      .Where(x => x.PostId == postId)
                     .OrderByDescending(x => x.CreatedAt)
                     .ToListAsync();

            return comments.Select(CommentDtoExtensions.FromEntity).ToList();
        }

        public async Task<CommentEntity?> DbGetCommentByIdAsync(int commentId)
        {
            return await _context.Comments.SingleOrDefaultAsync(x => x.Id == commentId);
        }
        public async Task DbAddCommentAsync(CommentEntity Comment)
        { 
            _context.Comments.Add(Comment);
            await _context.SaveChangesAsync();
        }
        public async Task DbDeleteCommentAsync(int commentId)
        {
            var comment = await DbGetCommentByIdAsync(commentId);
            if (comment == null)
            {
                return;
            }
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }
    }
}
