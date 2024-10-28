using System;

namespace Reservations {

    public class TestReservationInsurance {

        public int Id { get; set; }
        public Guid ReservationId { get; set; }
        public string InsuranceCompany { get; set; }
        public string PolicyNo { get; set; }
        public string PolicyEnds { get; set; }

    }

}