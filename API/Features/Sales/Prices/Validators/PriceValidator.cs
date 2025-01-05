using API.Infrastructure.Helpers;
using FluentValidation;

namespace API.Features.Sales.Prices {

    public class PriceValidator : AbstractValidator<PriceWriteDto> {

        public PriceValidator() {
            RuleFor(x => x.Description).NotEmpty().MaximumLength(10);
            RuleFor(x => x.LongDescription).MaximumLength(100);
            RuleFor(x => x.FromDate).Must(DateHelpers.BeCorrectFormat);
            RuleFor(x => x.ToDate).Must(DateHelpers.BeCorrectFormat);
            RuleFor(x => x.Amount).InclusiveBetween(0, 99999);
        }

    }

}