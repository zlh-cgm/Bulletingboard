using Bulletingboard.DTO.Post;
using System.Data;

namespace Bulletingboard.Requests.Post
{
    public class PostRequest
    {
        public int? Id { get; set; }
        public int? UserId { get; set; }
        public string Description { get; set; }
        public bool IsPrivate { get; set; } = false;

        public PostRequest()
        {
        }

        public PostRequest(PostDto postDto)
        {
            Id = postDto.Id;
            UserId = postDto.UserId;
            Description = postDto.Description;
            IsPrivate = postDto.IsPrivate;
        }
    }
}
