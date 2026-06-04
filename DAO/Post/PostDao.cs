using Bulletingboard.DTO.Post;
using Bulletingboard.DTO.User;
using Bulletingboard.Entity;
using Microsoft.EntityFrameworkCore;
using PostEntity = Bulletingboard.Entity.Post;

namespace Bulletingboard.DAO.Post
{
    public class PostDao:IPostDao
    {
        private readonly BulletingboardDbContext _context;

        public PostDao(BulletingboardDbContext context)
        {
            _context = context;
        }
        public async Task<List<PostDto>> DbGetPublicPostAsync(int userId) 
        {
            var posts = await _context.Posts
              .Where(p => !p.IsPrivate || p.UserId == userId)
              .Include(p => p.user)
              .Include(p=>p.Comments.OrderByDescending(c=>c.CreatedAt))
             .OrderByDescending(post => post.CreatedAt)
             .ToListAsync();

            return posts.Select(PostDtoExtensions.FromEntity).ToList();
        }
        public async Task<List<PostDto>> DbGetPostByUserIdAsync(int userId)
        {
            var posts = await _context.Posts
              .Where(p => p.UserId == userId)
             .OrderByDescending(post => post.CreatedAt)
             .ToListAsync();

            return posts.Select(PostDtoExtensions.FromEntity).ToList();
        }
        public async Task<List<PostDto>> DbGetPublicPostByUserIdAsync(int userId)
        {
            var posts = await _context.Posts
             .Where(p => p.UserId == userId && p.IsPrivate==false)
             .OrderByDescending(post => post.CreatedAt)
             .ToListAsync();

            return posts.Select(PostDtoExtensions.FromEntity).ToList();
        }
        public async Task DbAddPostAsync(PostEntity post)
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
        }

        public async Task<PostEntity?> DbGetPostByIdAsync(int postId)
        {
            return await _context.Posts.Include(p=>p.user).Include(p => p.Comments.OrderByDescending(c=>c.CreatedAt)).SingleOrDefaultAsync(post => post.Id == postId);
        }

        public async Task DbUpdatePostAsync(PostEntity post)
        {
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
        }

        public async Task DbDeletePostAsync(int postId)
        {
            var post = await DbGetPostByIdAsync(postId);
            if (post == null)
            {
                return;
            }
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
        }
    }
}
