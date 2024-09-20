using System;

namespace API.Features.Reservations {

    public class ReservationPier {

        public int Id { get; set; }
        public Guid ReservationId { get; set; }
        public string Description { get; set; }

    }

}