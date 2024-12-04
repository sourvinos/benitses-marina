using System;

namespace API.Features.Leases {

    public class LeasePdfInsuranceVM {

        public int Id { get; set; }
        public Guid? ReservationId { get; set; }
        public string InsuranceCompany { get; set; }
        public string PolicyNo { get; set; }
        public string PolicyEnds { get; set; }
        
    }

}