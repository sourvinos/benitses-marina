using FluentValidation;

namespace API.Featuers.Sales.SeasonTypes {

    public class SeasonTypeValidation : AbstractValidator<SeasonTypeWriteDto> {

        public SeasonTypeValidation() {
            // Fields
            RuleFor(x => x.Description).NotEmpty().MaximumLength(128);
        }

    }

}