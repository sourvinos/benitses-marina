using System;

namespace API.Features.Leases {

    public class LeasePdfVM {

        public Guid ReservationId { get; set; }
        public string BackgroundColor { get; set; }
        public LeasePdfPeriodVM Period { get; set; }
        public LeasePdfBoatVM Boat { get; set; }
        public LeasePdfInsuranceVM Insurance { get; set; }
        public LeasePdfPersonVM Owner { get; set; }
        public LeasePdfPersonVM Billing { get; set; }
        public LeasePdfFeeVM Fee { get; set; }

    }

}