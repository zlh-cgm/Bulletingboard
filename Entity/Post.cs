namespace Bulletingboard.Entity
{
    public class Post
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public User user { get; set; }
        public bool IsPrivate { get; set; }=false;
        public List<Comment>? Comments { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
