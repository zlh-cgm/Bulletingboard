using FluentValidation;
using Bulletingboard.Requests.Auth;

namespace Bulletingboard.FluentValidators
{
    public class ResetPasswordRequestValidator:AbstractValidator<ResetPasswordRequest>
    {
        public ResetPasswordRequestValidator() 
        {
            RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("New password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Please confirm your new password.")
                .Equal(x => x.NewPassword).WithMessage("The new password and confirmation password do not match.");
        }
    }
}
