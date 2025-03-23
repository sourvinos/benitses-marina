using FluentValidation;

namespace API.Features.Cashiers.Safes {

    public class SafeValidator : AbstractValidator<SafeWriteDto> {

        public SafeValidator() {
            RuleFor(x => x.Description).NotEmpty().MaximumLength(128);
        }

    }

}