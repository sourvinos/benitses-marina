using System;
using API.Features.Reservations.Piers;

namespace API.Features.Reservations {

    public class ReservationPier {

        public int Id { get; set; }
        public Guid ReservationId { get; set; }
        public int PierId { get; set; }
        public Pier Pier { get; set; }

    }

}