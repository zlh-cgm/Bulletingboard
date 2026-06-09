using Bulletingboard.DTO.User;
using Bulletingboard.Requests.User;
using UserEntity = Bulletingboard.Entity.User;

namespace Bulletingboard.DAO.User;

public interface IUserDao
{
    /// <summary>
    /// Retrieves a list of all users from the database
    /// </summary>
    /// <returns></returns>
    Task<List<UserDto>> DbGetUserInfoAsync();
    /// <summary>
    /// Checks the database if an email is already taken by another user
    /// </summary>
    /// <param name="userRequest">userRequest</param>
    /// <returns></returns>
    Task<bool> DbCheckDuplicateUserByEmailAndIdAsync(UserRequest userRequest);
    /// <summary>
    /// Checks the database if a username is already taken by another user.
    /// </summary>
    /// <param name="userRequest">userRequest</param>
    /// <returns></returns>
    Task<bool> DbCheckDuplicateUserByNameAndIdAsync(UserRequest userRequest);
    /// <summary>
    /// Inserts a new user into the database
    /// </summary>
    /// <param name="user">Id,Email,Password,Name,Img,Role,CreatedAt,UpdatedAt,ResetToken,ResetTokenExpireAt</param>
    /// <returns></returns>
    Task DbAddUserAsync(UserEntity user);
    /// <summary>
    /// Retrieves a single user entity by their ID from the database
    /// </summary>
    /// <param name="userId">userId</param>
    /// <returns></returns>
    Task<UserEntity?> DbGetUserByIdAsync(int userId);
    /// <summary>
    /// Updates an existing user's details in the database
    /// </summary>
    /// <param name="user">Id,Email,Password,Name,Img,Role,CreatedAt,UpdatedAt,ResetToken,ResetTokenExpireAt</param>
    /// <returns></returns>
    Task DbUpdateUserAsync(UserEntity user);
    /// <summary>
    /// Removes a specific user from the database by their ID
    /// </summary>
    /// <param name="userId">userId</param>
    /// <returns></returns>
    Task DbDeleteUserAsync(int userId);
    /// <summary>
    /// Retrieves a single user entity by their email address from the database
    /// </summary>
    /// <param name="email">Email</param>
    /// <returns></returns>
    Task<UserEntity?> DbGetUserByEmailAsync(string email);
}
