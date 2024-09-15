using API.Infrastructure.Helpers;
using FluentValidation;

namespace API.Features.Reservations.Bookings {

    public class BookingValidator : AbstractValidator<BookingWriteDto> {

        public BookingValidator() {
            RuleFor(x => x.BoatTypeId).NotEmpty();
            RuleFor(x => x.BoatName).NotEmpty().MaximumLength(128);
            RuleFor(x => x.BoatLength).NotEmpty();
            RuleFor(x => x.BoatWidth).NotEmpty();
            RuleFor(x => x.FromDate).Must(DateHelpers.BeCorrectFormat);
            RuleFor(x => x.ToDate).Must(DateHelpers.BeCorrectFormat).GreaterThan(x => x.FromDate);
            RuleFor(x => x.ContactDetails).MaximumLength(128);
            RuleFor(x => x.Email).Must(EmailHelpers.BeEmptyOrValidEmailAddress).MaximumLength(128);
            RuleFor(x => x.Remarks).MaximumLength(128);
        }

    }

}