using FluentValidation;

namespace API.Features.Expenses.Suppliers {

    public class SupplierValidator : AbstractValidator<SupplierWriteDto> {

        public SupplierValidator() {
            RuleFor(x => x.Description).NotEmpty().MaximumLength(128);
            RuleFor(x => x.LongDescription).NotEmpty().MaximumLength(128);
            RuleFor(x => x.BankDescription).NotEmpty().MaximumLength(128);
            RuleFor(x => x.VatNumber).NotEmpty().MaximumLength(36);
            RuleFor(x => x.Phones).MaximumLength(128);
            RuleFor(x => x.Email).MaximumLength(128);
            RuleFor(x => x.Iban).MaximumLength(30);
            RuleFor(x => x.Remarks).MaximumLength(2048);
        }

    }

}