namespace Bulletingboard.Requests.Comment
{
    public class CommentRequest
    {
        public int? Id { get; set; }
        public int PostId { get; set; }
        public string Content { get; set; }

    }
}
