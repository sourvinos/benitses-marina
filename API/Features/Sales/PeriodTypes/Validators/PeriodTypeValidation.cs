using FluentValidation;

namespace API.Featuers.Sales.PeriodTypes {

    public class PeriodTypeValidation : AbstractValidator<PeriodTypeWriteDto> {

        public PeriodTypeValidation() {
            // Fields
            RuleFor(x => x.Description).NotEmpty().MaximumLength(128);
        }

    }

}