using Bulletingboard.DTO.Auth;

namespace Bulletingboard.Services.Auth
{
    public interface IAuthService
    {
        /// <summary>
        /// Authenticates a user by checking their email and password.
        /// </summary>
        /// <param name="loginDto">Email,Password</param>
        /// <returns></returns>
        Task<LoginResultDto?> LoginAsync(LoginDto loginDto);
        /// <summary>
        /// Validates a password reset token
        /// </summary>
        /// <param name="id">user id</param>
        /// <param name="token">reset token</param>
        /// <returns></returns>
        Task<bool> ValidateResetLinkAsync(int id, string token);
        /// <summary>
        /// Changes the user's password
        /// </summary>
        /// <param name="resetPasswordDto">UserId,NewPassword,ConfirmPassword</param>
        /// <returns></returns>
        Task ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
        /// <summary>
        /// Sends a password reset email
        /// </summary>
        /// <param name="email">email</param>
        /// <returns></returns>
        Task<bool> SendEmailAsync(string email);
    }
}