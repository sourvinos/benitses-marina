using System;

namespace API.Features.LeaseAgreements {

    public class LeaseAgreementVM {

        public Guid ReservationId { get; set; }
        public LeaseAgreementPeriodVM Period { get; set; }
        public LeaseAgreementBoatVM Boat { get; set; }
        public LeaseAgreementInsuranceVM Insurance { get; set; }
        public LeaseAgreementPersonVM Owner { get; set; }
        public LeaseAgreementPersonVM Billing { get; set; }
        public LeaseAgreementFeeVM Fee { get; set; }

    }

}