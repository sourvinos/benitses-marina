namespace API.Features.Reservations {

    public interface IReservationValidation {

        int IsValid(Reservation x, ReservationWriteDto Reservation);

    }

}