using API.Infrastructure.Classes;

namespace API.Features.Reservations {

    public class ReservationPierVM {

        public int Id { get; set; }
        public string ReservationId { get; set; }
        public int PierId { get; set; }
        public SimpleEntity Pier { get; set; }

    }

}