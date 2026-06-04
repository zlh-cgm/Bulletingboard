using Bulletingboard.DTO.User;
using Bulletingboard.Requests.Auth;
using Bulletingboard.Requests.User;

namespace Bulletingboard.Services.User;

public interface IUserService
{
    Task<List<UserDto>> GetUserInfoListAsync();

    Task<bool> CheckDuplicateUserAsync(UserRequest userRequest, string type);

    Task AddUserAsync(UserDto userDto);

    Task<UserDto?> GetUserByIdAsync(int userId);

    Task UpdateUserAsync(UserDto userDto);

    Task DeleteUserAsync(int userId);
}
