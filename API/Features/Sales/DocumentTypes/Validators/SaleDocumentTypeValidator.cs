using FluentValidation;

namespace API.Features.Sales.DocumentTypes {

    public class SaleDocumentTypeValidator : AbstractValidator<SaleDocumentTypeWriteDto> {

        public SaleDocumentTypeValidator() {
            // Fields
            RuleFor(x => x.Abbreviation).NotEmpty().MaximumLength(5);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(128);
            RuleFor(x => x.Batch).NotNull().MaximumLength(5);
            RuleFor(x => x.DiscriminatorId).NotNull().InclusiveBetween(1, 3);
            RuleFor(x => x.Customers).NotNull().MaximumLength(1).Matches(@"^[+|\-| ]*$");
            RuleFor(x => x.Table8_1).NotNull().MaximumLength(32);
            RuleFor(x => x.Table8_8).NotNull().MaximumLength(32);
            RuleFor(x => x.Table8_9).NotNull().MaximumLength(32);
        }

    }

}