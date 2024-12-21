using FluentValidation;

namespace API.Features.Reservations.PaymentStatuses {

    public class PaymentStatusValidator : AbstractValidator<PaymentStatusWriteDto> {

        public PaymentStatusValidator() {
            RuleFor(x => x.Description).NotEmpty().MaximumLength(128);
        }

    }

}