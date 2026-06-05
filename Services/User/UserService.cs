using Bulletingboard.DAO.User;
using Bulletingboard.DTO.Auth;
using Bulletingboard.DTO.User;
using Bulletingboard.Requests.User;
using Microsoft.AspNetCore.Identity;
using UserEntity = Bulletingboard.Entity.User;

namespace Bulletingboard.Services.User;

public class UserService : IUserService
{
    private static readonly HashSet<string> AllowedImageTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "image/jpeg",
        "image/png",
        "image/gif",
        "image/webp"
    };

    private readonly IUserDao _userDao;
    private readonly IWebHostEnvironment _environment;
    private readonly IPasswordHasher<UserEntity> _passwordHasher;
    public UserService(IUserDao userDao, IWebHostEnvironment environment,IPasswordHasher<UserEntity> passwordHasher)
    {
        _userDao = userDao;
        _environment = environment;
        _passwordHasher = passwordHasher;
    }

    public async Task<List<UserDto>> GetUserInfoListAsync()
    {
        return await _userDao.DbGetUserInfoAsync();
    }

    public async Task<bool> CheckDuplicateUserAsync(UserRequest userRequest, string type)
    {
        return type == "email"
            ? await _userDao.DbCheckDuplicateUserByEmailAndIdAsync(userRequest)
            : await _userDao.DbCheckDuplicateUserByNameAndIdAsync(userRequest);
    }

    public async Task AddUserAsync(UserDto userDto)
    {
        userDto.Img = await SaveUserImageAsync(userDto.FileUpload);
        var user = userDto.BindDbModel();
        user.Password= _passwordHasher.HashPassword(user, user.Password);
        await _userDao.DbAddUserAsync(user);
        userDto.Id = user.Id;
    }

    public async Task<UserDto?> GetUserByIdAsync(int userId)
    {
        var user = await _userDao.DbGetUserByIdAsync(userId);
        return user is null ? null : UserDtoExtensions.FromEntity(user);
    }

    public async Task UpdateUserAsync(UserDto userDto)
    {
        if (userDto.Id is null)
        {
            throw new ArgumentException("User id is required.", nameof(userDto));
        }

        var user = await _userDao.DbGetUserByIdAsync(userDto.Id.Value);
        if (user is null)
        {
            return;
        }

        user.Name = userDto.Name.Trim();
        user.Email = userDto.Email.Trim();
        user.Role = userDto.Role;
        user.UpdatedAt = DateTime.Now;
        user.UpdatedBy = 1;

        var imageName = await SaveUserImageAsync(userDto.FileUpload);
        if (!string.IsNullOrWhiteSpace(imageName))
        {
            DeleteUserImage(user.Img);
            user.Img = imageName;
        }

        await _userDao.DbUpdateUserAsync(user);
    }

    public async Task<bool> ChangePasswordAsync(ChangePasswordRequest changePasswordRequest)
    {
        var user = await _userDao.DbGetUserByIdAsync(changePasswordRequest.Id);
        if (user is null)
        {
            return false;
        }

        var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, changePasswordRequest.OldPassword);

        if (verificationResult == PasswordVerificationResult.Failed)
        {
            return false;
        }


        user.Password = _passwordHasher.HashPassword(user, changePasswordRequest.NewPassword);

        await _userDao.DbUpdateUserAsync(user);
        return true;
    }

    public async Task DeleteUserAsync(int userId)
    {
        var user = await _userDao.DbGetUserByIdAsync(userId);
        if (user?.Img is not null)
        {
            DeleteUserImage(user.Img);
        }

        await _userDao.DbDeleteUserAsync(userId);
    }

    private async Task<string?> SaveUserImageAsync(IFormFile? file)
    {
        if (file is null || file.Length == 0)
        {
            return null;
        }

        if (!AllowedImageTypes.Contains(file.ContentType))
        {
            return null;
        }

        var extension = Path.GetExtension(file.FileName);
        var fileName = $"{Guid.NewGuid():N}{extension}";
        var uploadRoot = Path.Combine(_environment.WebRootPath, "uploads", "users");
        Directory.CreateDirectory(uploadRoot);
        var filePath = Path.Combine(uploadRoot, fileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        return fileName;
    }

    private void DeleteUserImage(string? imageName)
    {
        if (string.IsNullOrWhiteSpace(imageName))
        {
            return;
        }

        var filePath = Path.Combine(_environment.WebRootPath, "uploads", "users", imageName);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
}
