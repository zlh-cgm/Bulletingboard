using FluentValidation;
using Bulletingboard.Requests.Comment;

namespace Bulletingboard.FluentValidators
{
    public class CommentRequestValidator:AbstractValidator<CommentRequest>
    {
        public CommentRequestValidator()
        {
            RuleFor(x => x.PostId)
            .NotEmpty()
            .WithMessage("Need Post Id!");

            RuleFor(x => x.Content)
                .NotEmpty()
                .WithMessage("Comment can't be empty!")
                .MaximumLength(50)
                .WithMessage("Can't be more than 50 characters!");
        }
    }
}
