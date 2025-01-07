using FluentValidation;

namespace API.Features.Sales.Prices {

    public class PriceValidator : AbstractValidator<PriceWriteDto> {

        public PriceValidator() {
            RuleFor(x => x.Code).NotEmpty().MaximumLength(10);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(128);
            RuleFor(x => x.EnglishDescription).NotEmpty().MaximumLength(128);
            RuleFor(x => x.NetAmount).InclusiveBetween(0, 99999);
            RuleFor(x => x.VatPercent).InclusiveBetween(0, 100);
        }

    }

}