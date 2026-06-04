using Bulletingboard.DTO.Comment;
using Bulletingboard.DTO.Post;
using Bulletingboard.Requests.Post;
using PostEntity = Bulletingboard.Entity.Post;

namespace Bulletingboard.DTO.Post
{
    public class PostDto
    {
        public int? Id { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public List<CommentDto>? Comments { get; set; }
        public bool IsPrivate { get; set; } = false;
        public string? CreatedAt { get; set; }
        public string? UpdatedAt { get; set; }


        public PostDto() { }

        public PostDto(PostRequest postRequest)
        {
            Id = postRequest.Id;
            Description = postRequest.Description;
            IsPrivate = postRequest.IsPrivate;
        }
    }
}

public static class PostDtoExtensions
{
    public static PostEntity BindDbModel(this PostDto postDto)
    {
        return new PostEntity
        {
            UserId = postDto.UserId,
            Description = postDto.Description,
            IsPrivate = postDto.IsPrivate,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
        };
    }

    public static PostDto FromEntity(PostEntity post)
    {
        return new PostDto
        {
            Id = post.Id,
            Comments=post.Comments?.Select(CommentDtoExtensions.FromEntity).ToList(),
            UserId= post.UserId,
            UserName=post.user.Name,
            IsPrivate= post.IsPrivate,
            Description = post.Description,
            CreatedAt = post.CreatedAt?.ToString("yyyy-MM-dd HH:mm:ss"),
            UpdatedAt = post.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss")
        };
    }
}
