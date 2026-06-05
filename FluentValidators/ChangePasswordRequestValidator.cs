using FluentValidation;
using Bulletingboard.Requests.User;

namespace Bulletingboard.FluentValidators
{
    public class ChangePasswordRequestValidator:AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordRequestValidator()
        {
            RuleFor(x => x.OldPassword)
            .NotEmpty().WithMessage("Old password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 8 characters long.");

            RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("New password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 8 characters long.");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Please confirm your new password.")
                .Equal(x => x.NewPassword).WithMessage("The new password and confirmation password do not match.");
        }
    }
}
