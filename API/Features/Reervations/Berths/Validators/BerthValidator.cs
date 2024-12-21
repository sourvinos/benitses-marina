using FluentValidation;

namespace API.Features.Reservations.Berths {

    public class BerthValidator : AbstractValidator<BerthWriteDto> {

        public BerthValidator() {
            RuleFor(x => x.Description).NotEmpty().MaximumLength(128);
        }

    }

}