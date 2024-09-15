using FluentValidation;

namespace API.Features.Reservations.Piers {

    public class PierValidator : AbstractValidator<PierWriteDto> {

        public PierValidator() {
            RuleFor(x => x.Description).NotEmpty().MaximumLength(128);
        }

    }

}