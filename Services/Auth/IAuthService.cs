using Bulletingboard.DTO.Auth;

namespace Bulletingboard.Services.Auth
{
    public interface IAuthService
    {
        Task<LoginResultDto?> LoginAsync(LoginDto loginDto);
        Task<bool> ValidateResetLinkAsync(int id, string token);
        Task ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
        Task<bool> SendEmailAsync(string email);
    }
}