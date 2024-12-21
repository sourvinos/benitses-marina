using FluentValidation;

namespace API.Features.Expenses.Banks {

    public class BankValidator : AbstractValidator<BankWriteDto> {

        public BankValidator() {
            RuleFor(x => x.Description).NotEmpty().MaximumLength(128);
        }

    }

}