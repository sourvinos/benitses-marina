using System;

namespace API.Features.Reservations {

    public class ReservationLease {

        public int Id { get; set; }
        public Guid ReservationId { get; set; }
        public string InsuranceCompany { get; set; }
        public string PolicyNo { get; set; }
        public DateTime PolicyEnds { get; set; }

    }

}