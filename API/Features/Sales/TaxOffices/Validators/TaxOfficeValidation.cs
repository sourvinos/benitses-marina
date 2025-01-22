using FluentValidation;

namespace API.Features.Sales.TaxOffices {

    public class TaxOfficeValidation : AbstractValidator<TaxOfficeWriteDto> {

        public TaxOfficeValidation() {
            // Fields
            RuleFor(x => x.Description).NotEmpty().MaximumLength(128);
        }

    }

}