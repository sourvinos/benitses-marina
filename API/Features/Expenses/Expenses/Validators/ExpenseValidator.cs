using FluentValidation;

namespace API.Features.Expenses.Expenses {

    public class ExpenseValidator : AbstractValidator<ExpenseWriteDto> {

        public ExpenseValidator() {
            RuleFor(x => x.InvoiceNo).NotEmpty().MaximumLength(16);
        }

    }

}