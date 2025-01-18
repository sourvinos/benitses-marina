using System.Threading.Tasks;

namespace API.Features.Reservations.Transactions {

    public interface IReservationValidation {

        Task<int> IsValidAsync(Reservation x, ReservationWriteDto reservation);

    }

}