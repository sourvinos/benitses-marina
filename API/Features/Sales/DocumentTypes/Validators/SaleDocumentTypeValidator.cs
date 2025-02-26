using FluentValidation;

namespace API.Features.Sales.DocumentTypes {

    public class SaleDocumentTypeValidator : AbstractValidator<SaleDocumentTypeWriteDto> {

        public SaleDocumentTypeValidator() {
            RuleFor(x => x.Abbreviation).NotEmpty().MaximumLength(16);
            RuleFor(x => x.AbbreviationEn).NotEmpty().MaximumLength(16);
            RuleFor(x => x.AbbreviationDataUp).NotEmpty().MaximumLength(16);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(128);
            RuleFor(x => x.Batch).NotNull().MaximumLength(5);
            RuleFor(x => x.DiscriminatorId).NotNull().InclusiveBetween(1, 2);
            RuleFor(x => x.Customers).NotNull().MaximumLength(1).Matches(@"^[+|\-| ]*$");
        }

    }

}