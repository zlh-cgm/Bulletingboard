using FluentValidation;
using Bulletingboard.Requests.Post;
namespace Bulletingboard.FluentValidators
{
    public class PostRequestValidator:AbstractValidator<PostRequest>
    {
        public PostRequestValidator()
        {
            RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Write Something Sir!")
            .MaximumLength(500)
            .WithMessage("Can't be more than 500 characters!");
        }
    }
}
