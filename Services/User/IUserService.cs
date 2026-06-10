using Bulletingboard.DTO.User;
using Bulletingboard.Requests.User;

namespace Bulletingboard.Services.User;

public interface IUserService
{
    /// <summary>
    /// Gets a list of all users
    /// </summary>
    /// <returns></returns>
    Task<List<UserDto>> GetUserInfoListAsync();
    /// <summary>
    /// Checks if a user's details (like email or username) already exist based on the check type
    /// </summary>
    /// <param name="userRequest">userRequest</param>
    /// <param name="type">email or name</param>
    /// <returns></returns>
    Task<bool> CheckDuplicateUserAsync(UserRequest userRequest, string type);
    /// <summary>
    /// Creates a single new user
    /// </summary>
    /// <param name="userDto">Name,Email,Password,Role,FileUpload,Img</param>
    /// <returns></returns>
    Task AddUserAsync(UserDto userDto);
    /// <summary>
    /// Creates multiple new users at once
    /// </summary>
    /// <param name="users">List of Name,Email,Password</param>
    /// <returns></returns>
    Task AddUserListAsync(List<UserRequest> users);
    /// <summary>
    /// Gets a single user's profile by their ID
    /// </summary>
    /// <param name="userId">userId</param>
    /// <returns></returns>
    Task<UserDto?> GetUserByIdAsync(int userId);
    /// <summary>
    /// Updates a user's account details
    /// </summary>
    /// <param name="userDto">Id,Name,Email,Role,Img,FileUpload,</param>
    /// <returns></returns>
    Task UpdateUserAsync(UserDto userDto);
    /// <summary>
    /// Changes a user's password
    /// </summary>
    /// <param name="changePasswordRequest">Id,OldPassword,NewPassword,ConfirmPassword</param>
    /// <returns></returns>
    Task ChangePasswordAsync(ChangePasswordRequest changePasswordRequest);
    /// <summary>
    /// Deletes a specific user by their ID
    /// </summary>
    /// <param name="userId">userId</param>
    /// <returns></returns>
    Task DeleteUserAsync(int userId);
    /// <summary>
    /// Upload user CSV file and Add users
    /// </summary>
    /// <param name="uploadCSVRequest">CSVFile</param>
    /// <returns></returns>
    Task UploadUserListCSVAsync(UploadCSVRequest uploadCSVRequest);
    /// <summary>
    /// Download user list CSV file
    /// </summary>
    /// <returns></returns>
    Task<byte[]> DownloadUserListCSVAsync();
}
