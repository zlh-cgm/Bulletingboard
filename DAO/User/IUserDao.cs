using Bulletingboard.DTO.User;
using Bulletingboard.Requests.User;
using UserEntity = Bulletingboard.Entity.User;

namespace Bulletingboard.DAO.User;

public interface IUserDao
{
    Task<List<UserDto>> DbGetUserInfoAsync();

    Task<bool> DbCheckDuplicateUserByEmailAndIdAsync(UserRequest userRequest);

    Task<bool> DbCheckDuplicateUserByNameAndIdAsync(UserRequest userRequest);

    Task DbAddUserAsync(UserEntity user);

    Task<UserEntity?> DbGetUserByIdAsync(int userId);

    Task DbUpdateUserAsync(UserEntity user);

    Task DbDeleteUserAsync(int userId);

    Task<UserEntity?> DbGetUserByEmailAsync(string email);
}
