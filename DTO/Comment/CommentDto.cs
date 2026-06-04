using Bulletingboard.Requests.Comment;
using Bulletingboard.Requests.Post;
using CommentEntity = Bulletingboard.Entity.Comment;

namespace Bulletingboard.DTO.Comment
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string CreatedAt { get; set; }

        public CommentDto() { }

        public CommentDto(CommentRequest commentRequest)
        {
            PostId= commentRequest.PostId;
            Content = commentRequest.Content;
        }
    }

    public static class CommentDtoExtensions
    {
        public static CommentEntity BindDbModel(this CommentDto CommentDto)
        {
            return new CommentEntity
            {
                Id = CommentDto.Id,
                Content = CommentDto.Content,
                PostId = CommentDto.PostId,
                UserId = CommentDto.UserId,
                UserName = CommentDto.UserName,
                CreatedAt = DateTime.Now,
            };
        }

        public static CommentDto FromEntity(CommentEntity comment)
        {
            return new CommentDto
            {
                Id = comment.Id,
                Content= comment.Content,
                UserId = comment.UserId,
                UserName = comment.UserName,
                PostId= comment.PostId,
                CreatedAt = comment.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
            };
        }
    }
}
