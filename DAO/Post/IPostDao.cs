using Bulletingboard.DTO.Post;
using PostEntity = Bulletingboard.Entity.Post;

namespace Bulletingboard.DAO.Post
{
    public interface IPostDao
    {
        /// <summary>
        /// Retrieves all public posts and private posts of a specific user from the database (including comments)
        /// </summary>
        /// <param name="userId">userId</param>
        /// <returns></returns>
        Task<List<PostDto>> DbGetPublicPostAsync(int userId);
        /// <summary>
        /// Retrieves all posts (public and private) created by a specific user from the database
        /// </summary>
        /// <param name="userId">userId</param>
        /// <returns></returns>
        Task<List<PostDto>> DbGetPostByUserIdAsync(int userId);
        /// <summary>
        /// Retrieves only the public posts created by a specific user from the database
        /// </summary>
        /// <param name="userId">userId</param>
        /// <returns></returns>
        Task<List<PostDto>> DbGetPublicPostByUserIdAsync(int userId);
        /// <summary>
        /// Inserts a new post into the database
        /// </summary>
        /// <param name="post">Id,Description,UserId,IsPrivate,CreatedAt,UpdatedAt</param>
        /// <returns></returns>
        Task DbAddPostAsync(PostEntity post);
        /// <summary>
        /// Retrieves a single post entity by its ID from the database (including comments)
        /// </summary>
        /// <param name="postId">postId</param>
        /// <returns></returns>
        Task<PostEntity?> DbGetPostByIdAsync(int postId);
        /// <summary>
        /// Updates an existing post's details in the database
        /// </summary>
        /// <param name="post">Id,Description,UserId,IsPrivate,CreatedAt,UpdatedAt</param>
        /// <returns></returns>
        Task DbUpdatePostAsync(PostEntity post);
        /// <summary>
        /// Removes a specific post from the database by its ID
        /// </summary>
        /// <param name="postId">postId</param>
        /// <returns></returns>
        Task DbDeletePostAsync(int postId);
    }
}
