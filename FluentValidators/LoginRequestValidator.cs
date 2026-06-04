using Bulletingboard.Requests.Auth;
using FluentValidation;
namespace Bulletingboard.FluentValidators
{
    public class LoginRequestValidator:AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator() 
        {
            RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email PLZ!!!")
            .EmailAddress()
            .WithMessage("Hey! WRONG Email format");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password PLZ!!!")
                .MinimumLength(6)
                .WithMessage("At least 6 characters SIR!!!");
        }
    }
}
