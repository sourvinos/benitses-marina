namespace API.Features.Reservations.Bookings {

    public interface IBookingValidation {

        int IsValid(Booking x, BookingWriteDto booking);

    }

}