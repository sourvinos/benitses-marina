using System;

namespace Reservations {

    public class TestReservationBerth {

        public int Id { get; set; }
        public Guid ReservationId { get; set; }
        public string Description { get; set; }
        public TestReservation Reservation { get; set; }

    }

}