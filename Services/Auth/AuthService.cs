using Bulletingboard.DAO.User;
using Bulletingboard.DTO.Auth;
using Bulletingboard.DTO.User;
using Microsoft.AspNetCore.Identity;
using UserEntity = Bulletingboard.Entity.User;

namespace Bulletingboard.Services.Auth
{
    public class AuthService:IAuthService
    {
        private readonly IUserDao _userDao;
        private readonly IPasswordHasher<UserEntity> _passwordHasher;

        public AuthService(IUserDao userDao, IPasswordHasher<UserEntity> passwordHasher)
        {
            _userDao = userDao;
            _passwordHasher = passwordHasher;
        }

        public async Task<LoginResultDto?> LoginAsync(LoginDto loginDto)
        {
            var user = await _userDao.DbGetUserByEmailAsync(loginDto.Email);

            if (user == null)
            {
                return null;
            }

            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, loginDto.Password);

            if (verificationResult == PasswordVerificationResult.Failed)
            {
                return null;
            }

            var RoleName = user.Role == 1 ? "Admin" : "Member";
            var result = new LoginResultDto()
            {
                Username=user.Name,
                Email = user.Email,
                UserId = user.Id,
                Role = RoleName
            };
            return result;
        }
    }
}
