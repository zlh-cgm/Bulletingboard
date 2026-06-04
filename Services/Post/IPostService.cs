using Bulletingboard.DTO.Post;

namespace Bulletingboard.Services.Post
{
    public interface IPostService
    {
        Task<List<PostDto>> GetPublicPostListAsync(int userId);

        Task<List<PostDto>> GetPostByUserIdAsync(int userId);
        Task<List<PostDto>> GetPublicPostByUserIdAsync(int userId);
        Task AddPostAsync(PostDto postDto);

        Task<PostDto?> GetPostByIdAsync(int postId);

        Task UpdatePostAsync(PostDto postDto);

        Task DeletePostAsync(int postId);
    }
}
