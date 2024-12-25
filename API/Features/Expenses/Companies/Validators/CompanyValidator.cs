using FluentValidation;

namespace API.Features.Expenses.Companies {

    public class CompanyValidator : AbstractValidator<CompanyWriteDto> {

        public CompanyValidator() {
            RuleFor(x => x.Description).NotEmpty().MaximumLength(128);
        }

    }

}