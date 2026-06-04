using Bulletingboard.DTO.User;
using Bulletingboard.Entity;
using Bulletingboard.Requests.User;
using Microsoft.EntityFrameworkCore;
using UserEntity = Bulletingboard.Entity.User;

namespace Bulletingboard.DAO.User;

public class UserDao : IUserDao
{
    private readonly BulletingboardDbContext _context;

    public UserDao(BulletingboardDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserDto>> DbGetUserInfoAsync()
    {
        var users = await _context.Users
            .OrderByDescending(user => user.CreatedAt)
            .ToListAsync();

        return users.Select(UserDtoExtensions.FromEntity).ToList();
    }

    public async Task<bool> DbCheckDuplicateUserByEmailAndIdAsync(UserRequest userRequest)
    {
        return await GetDuplicateUserQuery(userRequest, "email").AnyAsync();
    }

    public async Task<bool> DbCheckDuplicateUserByNameAndIdAsync(UserRequest userRequest)
    {
        return await GetDuplicateUserQuery(userRequest, "name").AnyAsync();
    }

    public async Task DbAddUserAsync(UserEntity user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task<UserEntity?> DbGetUserByIdAsync(int userId)
    {
        return await _context.Users.SingleOrDefaultAsync(user => user.Id == userId);
    }

    public async Task DbUpdateUserAsync(UserEntity user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DbDeleteUserAsync(int userId)
    {
        var user = await DbGetUserByIdAsync(userId);
        if (user is null)
        {
            return;
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task<UserEntity?> DbGetUserByEmailAsync(string email)
    {
        return await _context.Users.SingleOrDefaultAsync(user => user.Email == email);
 
    }

    private IQueryable<UserEntity> GetDuplicateUserQuery(UserRequest userRequest, string type)
    {
        var query = _context.Users.AsQueryable();

        if (type == "email" && !string.IsNullOrWhiteSpace(userRequest.Email))
        {
            query = query.Where(user => user.Email == userRequest.Email.Trim());
        }
        else if (type == "name" && !string.IsNullOrWhiteSpace(userRequest.Name))
        {
            query = query.Where(user => user.Name == userRequest.Name.Trim());
        }
        else
        {
            return _context.Users.Where(user => false);
        }

        if (userRequest.Id is > 0)
        {
            query = query.Where(user => user.Id != userRequest.Id);
        }

        return query;
    }
}
