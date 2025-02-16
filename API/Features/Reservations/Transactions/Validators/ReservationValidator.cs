using API.Infrastructure.Helpers;
using FluentValidation;

namespace API.Features.Reservations.Transactions {

    public class ReservationValidator : AbstractValidator<ReservationWriteDto> {

        public ReservationValidator() {
            RuleFor(x => x.Boat.Name).NotEmpty().MaximumLength(128);
            RuleFor(x => x.Boat.Loa).NotEmpty();
            RuleFor(x => x.FromDate).Must(DateHelpers.BeCorrectFormat);
            RuleFor(x => x.ToDate).Must(DateHelpers.BeCorrectFormat).GreaterThan(x => x.FromDate);
            RuleFor(x => x.Remarks).MaximumLength(2048);
            RuleFor(x => x.FinancialRemarks).MaximumLength(2048);
        }

    }

}