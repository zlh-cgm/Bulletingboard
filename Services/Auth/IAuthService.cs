using Bulletingboard.DTO.Auth;

namespace Bulletingboard.Services.Auth
{
    public interface IAuthService
    {
        public Task<LoginResultDto?> LoginAsync(LoginDto loginDto);
    }
}