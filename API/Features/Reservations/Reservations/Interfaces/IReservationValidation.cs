using System.Threading.Tasks;

namespace API.Features.Reservations {

    public interface IReservationValidation {

        Task<int> IsValidAsync(Reservation x, ReservationWriteDto reservation);

    }

}