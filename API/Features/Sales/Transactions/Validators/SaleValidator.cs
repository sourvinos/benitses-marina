using FluentValidation;

namespace API.Features.Sales.Transactions {

    public class SaleValidator : AbstractValidator<SaleWriteDto> {

        public SaleValidator() {
            RuleFor(x => x.CustomerId).NotEmpty();
            RuleFor(x => x.DocumentTypeId).NotEmpty();
            RuleFor(x => x.PaymentMethodId).NotEmpty();
            RuleFor(x => x.Remarks).MaximumLength(128);
        }

    }

}