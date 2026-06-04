using Bulletingboard.DTO.Post;
using PostEntity = Bulletingboard.Entity.Post;

namespace Bulletingboard.DAO.Post
{
    public interface IPostDao
    {
        Task<List<PostDto>> DbGetPublicPostAsync(int userId);
        Task<List<PostDto>> DbGetPostByUserIdAsync(int userId);
        Task<List<PostDto>> DbGetPublicPostByUserIdAsync(int userId);
        Task DbAddPostAsync(PostEntity post);
        Task<PostEntity?> DbGetPostByIdAsync(int postId);
        Task DbUpdatePostAsync(PostEntity post);
        Task DbDeletePostAsync(int postId);
    }
}
