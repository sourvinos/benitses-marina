using System;

namespace API.Features.LeaseAgreements {

    public class LeaseAgreementInsuranceVM {

        public int Id { get; set; }
        public Guid? ReservationId { get; set; }
        public string InsuranceCompany { get; set; }
        public string PolicyNo { get; set; }
        public string PolicyEnds { get; set; }
        
    }

}