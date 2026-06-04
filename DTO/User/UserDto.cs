using Bulletingboard.Constraints;
using Bulletingboard.Requests.User;
using UserEntity = Bulletingboard.Entity.User;

namespace Bulletingboard.DTO.User;

public class UserDto
{
    public int? Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? Password { get; set; }

    public int Role { get; set; }

    public string? Img { get; set; }

    public string CreatedAt { get; set; } = string.Empty;

    public string? UpdatedAt { get; set; }

    public IFormFile? FileUpload { get; set; }

    public UserDto()
    {
    }

    public UserDto(UserRequest userRequest)
    {
        Id = userRequest.Id;
        Name = userRequest.Name;
        Email = userRequest.Email;
        Password = userRequest.Password;
        Role = userRequest.Role;
        Img = userRequest.Img;
        FileUpload = userRequest.FileUpload;
    }
}

public static class UserDtoExtensions
{
    public static UserEntity BindDbModel(this UserDto userDto)
    {
        return new UserEntity
        {
            Name = userDto.Name.Trim(),
            Email = userDto.Email.Trim(),
            Password = userDto.Password?.Trim() ?? string.Empty,
            Role = userDto.Role == 0 ? UserRoles.Member : userDto.Role,
            Img = userDto.Img,
            CreatedAt = DateTime.Now,
            CreatedBy = 1,
            UpdatedAt = DateTime.Now,
            UpdatedBy = 1
        };
    }

    public static UserDto FromEntity(UserEntity user)
    {
        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role,
            Img = user.Img,
            CreatedAt = user.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
            UpdatedAt = user.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss")
        };
    }
}
