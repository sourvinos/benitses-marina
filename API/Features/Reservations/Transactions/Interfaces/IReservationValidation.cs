using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.Transactions {

    public interface IReservationValidation : IRepository<Reservation> {

        Task<int> IsValidAsync(Reservation x, ReservationWriteDto reservation);

    }

}