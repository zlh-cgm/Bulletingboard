using FluentValidation;
using Bulletingboard.Requests.Auth;
namespace Bulletingboard.FluentValidators
{
    public class ForgotPasswordRequestValidator:AbstractValidator<ForgotPasswordRequest>
    {
        public ForgotPasswordRequestValidator() 
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email PLZ!!!")
                .EmailAddress()
                .WithMessage("Hey! WRONG Email format");
        }
    }
}
