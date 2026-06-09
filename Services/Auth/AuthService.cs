using Bulletingboard.DAO.User;
using Bulletingboard.DTO.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using UserEntity = Bulletingboard.Entity.User;

namespace Bulletingboard.Services.Auth
{
    public class AuthService:IAuthService
    {
        private readonly IUserDao _userDao;
        private readonly IPasswordHasher<UserEntity> _passwordHasher;
        private readonly IEmailSender _emailSender;

        public AuthService(IUserDao userDao, IPasswordHasher<UserEntity> passwordHasher, IEmailSender emailSender)
        {
            _userDao = userDao;
            _passwordHasher = passwordHasher;
            _emailSender = emailSender;
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
        public async Task SendEmailAsync(string email)
        { 
            var user=await _userDao.DbGetUserByEmailAsync(email);
            if (user == null) 
            {
                throw new InvalidDataException("There's no account link to this Email");
            }
            string token = Guid.NewGuid().ToString();
            user.ResetToken=token;
            user.ResetTokenExpireAt = DateTime.Now.AddMinutes(10);
            string callbackUrl = $"https://localhost:7298/Auth/Reset/?userId={user.Id}&token={token}";
            await _emailSender.SendEmailAsync(email, "Reset Password", callbackUrl);
            await _userDao.DbUpdateUserAsync(user);
        }

        public async Task ValidateResetLinkAsync(int id, string token)
        { 
            var user=await _userDao.DbGetUserByIdAsync(id);
            if (user==null || user.ResetToken!=token)
            {
                throw new InvalidDataException("Invalid token.");
            }
            if (DateTime.Now > user.ResetTokenExpireAt)
            {
                throw new InvalidDataException("Password reset link expired.");
            }
        }

        public async Task ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            var user=await _userDao.DbGetUserByIdAsync(resetPasswordDto.UserId);
            if (user == null)
            {
                return;
            }

            user.Password = _passwordHasher.HashPassword(user, resetPasswordDto.NewPassword);
            user.ResetToken = null;
            user.ResetTokenExpireAt=null;
            await _userDao.DbUpdateUserAsync(user);

        }
    }
}
