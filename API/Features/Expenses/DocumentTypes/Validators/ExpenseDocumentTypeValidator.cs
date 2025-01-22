using FluentValidation;

namespace API.Features.Expenses.DocumentTypes {

    public class ExpenseDocumentTypeValidator : AbstractValidator<ExpenseDocumentTypeWriteDto> {

        public ExpenseDocumentTypeValidator() {
            RuleFor(x => x.Description).NotEmpty().MaximumLength(128);
        }

    }

}