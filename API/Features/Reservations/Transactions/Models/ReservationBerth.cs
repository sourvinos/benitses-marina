using System;

namespace API.Features.Reservations.Transactions {

    public class ReservationBerth {

        public int Id { get; set; }
        public Guid ReservationId { get; set; }
        public string Description { get; set; }
        public Reservation Reservation { get; set; }

    }

}