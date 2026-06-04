using Bulletingboard.DTO.Post;
using Bulletingboard.DTO.User;

namespace Bulletingboard.ViewModels.User;

public class UserDetailViewModel
{
    public UserDto User { get; set; } = new();
    public List<PostDto> Posts { get; set; }
}
