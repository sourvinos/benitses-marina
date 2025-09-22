using System;

namespace API.Features.Reservations.Transactions {

    public class ReservationFishingLicence {

        public int Id { get; set; }
        public Guid ReservationId { get; set; }
        public string IssuingAuthority { get; set; }
        public string LicenceNo { get; set; }
        public DateTime LicenceEnds { get; set; }

    }

}