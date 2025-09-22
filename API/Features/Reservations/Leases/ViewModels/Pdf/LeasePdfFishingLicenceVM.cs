using System;

namespace API.Features.Leases {

    public class LeasePdfFishingLicenceVM {

        public int Id { get; set; }
        public Guid? ReservationId { get; set; }
        public string IssuingAuthority { get; set; }
        public string LicenceNo { get; set; }
        public string LicenceEnds { get; set; }

    }

}