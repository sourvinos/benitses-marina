using FluentValidation;

namespace API.Features.BoatTypes {

    public class BoatTypeValidator : AbstractValidator<BoatTypeWriteDto> {

        public BoatTypeValidator() {
            RuleFor(x => x.Description).NotEmpty().MaximumLength(128);
        }

    }

}