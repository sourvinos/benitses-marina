using System;

namespace API.Features.Reservations.Transactions {

    public class ReservationFishingLicenceDetailDto {

        public int Id { get; set; }
        public Guid? ReservationId { get; set; }
        public string IssuingAuthority { get; set; }
        public string LicenceNo { get; set; }
        public string LicenceEnds { get; set; }
        
    }

}