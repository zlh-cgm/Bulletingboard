using FluentValidation;
using Bulletingboard.Requests.User;

namespace Bulletingboard.FluentValidators
{
    public class UploadCSVRequestValidator:AbstractValidator<UploadCSVRequest>
    {
        public UploadCSVRequestValidator() 
        {
            RuleFor(x => x.CSVFile)
                .NotEmpty()
                .WithMessage("Please upload a CSV file sir!")
                .Must(file => Path.GetExtension(file.FileName).Equals(".csv", System.StringComparison.OrdinalIgnoreCase))
                .WithMessage("Only CSV files are allowed!");
        }
    }
}
