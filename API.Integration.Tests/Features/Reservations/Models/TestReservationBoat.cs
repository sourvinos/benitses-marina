using System;

namespace Reservations {

    public class TestReservationBoat {

        public int Id { get; set; }
        public Guid ReservationId { get; set; }
        public int TypeId { get; set; }
        public int UsageId { get; set; }
        public string Name { get; set; }
        public string Loa { get; set; }
        public string Beam { get; set; }
        public string Draft { get; set; }

    }

}