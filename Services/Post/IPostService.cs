using Bulletingboard.DTO.Post;

namespace Bulletingboard.Services.Post
{
    public interface IPostService
    {
        /// <summary>
        /// Gets all public posts and private post of a specific user
        /// </summary>
        /// <param name="userId">userId</param>
        /// <returns></returns>
        Task<List<PostDto>> GetPublicPostListAsync(int userId);
        /// <summary>
        /// Gets all posts (public and private) created by a specific user
        /// </summary>
        /// <param name="userId">userId</param>
        /// <returns></returns>
        Task<List<PostDto>> GetPostByUserIdAsync(int userId);
        /// <summary>
        /// Gets only the public posts created by a specific user
        /// </summary>
        /// <param name="userId">userId</param>
        /// <returns></returns>
        Task<List<PostDto>> GetPublicPostByUserIdAsync(int userId);
        /// <summary>
        /// Creates a new post
        /// </summary>
        /// <param name="postDto">UserId,Description,IsPrivate</param>
        /// <returns></returns>
        Task AddPostAsync(PostDto postDto);
        /// <summary>
        /// Gets a single post by its ID
        /// </summary>
        /// <param name="postId">postId</param>
        /// <returns></returns>
        Task<PostDto?> GetPostByIdAsync(int postId);
        /// <summary>
        /// Updates an existing post's details
        /// </summary>
        /// <param name="postDto">Id,UserId,Description,IsPrivate</param>
        /// <returns></returns>
        Task UpdatePostAsync(PostDto postDto);
        /// <summary>
        /// Deletes a specific post by its ID
        /// </summary>
        /// <param name="postId">PostId</param>
        /// <returns></returns>
        Task DeletePostAsync(int postId);
    }
}
