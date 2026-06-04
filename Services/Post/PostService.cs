using Bulletingboard.DAO.Post;
using Bulletingboard.DTO.Post;
using Bulletingboard.DTO.User;
using System.Text.Json;
namespace Bulletingboard.Services.Post
{
    public class PostService:IPostService
    {
        private readonly IPostDao _postDao;
        private readonly ILogger<PostService> _logger;

        public PostService(IPostDao postDao, ILogger<PostService> logger)
        {
            _postDao = postDao;
            _logger = logger;
        }

        public async Task<List<PostDto>> GetPublicPostListAsync(int userId)
        { 
            return await _postDao.DbGetPublicPostAsync(userId);
        }

        public async Task<List<PostDto>> GetPostByUserIdAsync(int userId)
        {
            return await _postDao.DbGetPostByUserIdAsync(userId);
        }

        public async Task<List<PostDto>> GetPublicPostByUserIdAsync(int userId)
        {
            return await _postDao.DbGetPublicPostByUserIdAsync(userId);
        }
        public async Task AddPostAsync(PostDto postDto)
        {
            var postEntity = postDto.BindDbModel();

            await _postDao.DbAddPostAsync(postEntity);
        }

        public async Task<PostDto?> GetPostByIdAsync(int postId)
        {
            var post = await _postDao.DbGetPostByIdAsync(postId);
            return post is null ? null : PostDtoExtensions.FromEntity(post);
        }

        public async Task UpdatePostAsync(PostDto postDto)
        {
            if (postDto.Id is null)
            {
                throw new ArgumentException("User id is required.");
            }

            var post = await _postDao.DbGetPostByIdAsync(postDto.Id.Value);
            if (post is null)
            {
                return;
            }

            post.Description = postDto.Description.Trim();
            post.IsPrivate = postDto.IsPrivate;
            post.UpdatedAt = DateTime.Now;

            await _postDao.DbUpdatePostAsync(post);

        }

        public async Task DeletePostAsync(int postId)
        { 
            await _postDao.DbDeletePostAsync(postId);
        }
    }
}
