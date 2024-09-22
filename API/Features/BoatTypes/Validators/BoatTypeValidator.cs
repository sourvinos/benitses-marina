using FluentValidation;

namespace API.Features.Reservations.BoatTypes {

    public class BoatTypeValidator : AbstractValidator<BoatTypeWriteDto> {

        public BoatTypeValidator() {
            // Fields
            RuleFor(x => x.Description).NotEmpty().MaximumLength(128);
        }

    }

}