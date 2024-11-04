using System;

namespace API.Features.LeaseAgreements {

    public class LeaseAgreementPeriodVM {

        public Guid? ReservationId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }

    }

}