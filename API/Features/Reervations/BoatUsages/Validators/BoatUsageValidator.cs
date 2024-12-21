using FluentValidation;

namespace API.Features.BoatUsages {

    public class BoatUsageValidator : AbstractValidator<BoatUsageWriteDto> {

        public BoatUsageValidator() {
            RuleFor(x => x.Description).NotEmpty().MaximumLength(128);
        }

    }

}