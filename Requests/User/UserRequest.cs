using System.ComponentModel.DataAnnotations;
using Bulletingboard.Constraints;
using Bulletingboard.DTO.User;
using CsvHelper.Configuration.Attributes;
namespace Bulletingboard.Requests.User;

public class UserRequest : IValidatableObject
{
    [Optional]
    public int? Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [StringLength(255, MinimumLength = 6)]
    public string? Password { get; set; }

    [Optional]
    [Display(Name = "Confirm Password")]
    public string? ConfirmPassword { get; set; }

    [Optional]
    [Display(Name = "Role")]
    [Range(UserRoles.Admin, UserRoles.Member)]
    public int Role { get; set; } = UserRoles.Member;

    [Optional]
    public string? Img { get; set; }

    [Ignore]
    [Display(Name = "Profile Image")]
    public IFormFile? FileUpload { get; set; }

    public UserRequest()
    {
    }

    public UserRequest(UserDto userDto)
    {
        Id = userDto.Id;
        Name = userDto.Name;
        Email = userDto.Email;
        Role = userDto.Role;
        Img = userDto.Img;
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Id is null && string.IsNullOrWhiteSpace(Password))
        {
            yield return new ValidationResult("Password is required.", new[] { nameof(Password) });
        }

        if (!string.IsNullOrWhiteSpace(Password) && Password != ConfirmPassword)
        {
            yield return new ValidationResult("Password and confirmation password do not match.", new[] { nameof(ConfirmPassword) });
        }
    }
}
