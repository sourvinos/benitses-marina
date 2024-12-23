using FluentValidation;

namespace API.Features.Expenses.Invoices {

    public class InvoiceValidator : AbstractValidator<InvoiceWriteDto> {

        public InvoiceValidator() {
            RuleFor(x => x.DocumentNo).NotEmpty().MaximumLength(16);
        }

    }

}