namespace Bulletingboard.Entity;

public class User
{
    public int Id { get; set; }

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? Img { get; set; }

    public int Role { get; set; }

    public int CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
    public string? ResetToken { get; set; }
    public DateTime? ResetTokenExpireIn { get; set; }
    public List<Post> posts { get; set; }
}
