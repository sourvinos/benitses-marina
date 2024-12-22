using FluentValidation;

namespace API.Features.Expenses.Expenses {

    public class ExpenseValidator : AbstractValidator<ExpenseWriteDto> {

        public ExpenseValidator() {
            RuleFor(x => x.DocumentNo).NotEmpty().MaximumLength(16);
        }

    }

}