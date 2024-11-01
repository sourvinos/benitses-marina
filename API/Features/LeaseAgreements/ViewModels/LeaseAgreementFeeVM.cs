using System;

namespace API.Features.LeaseAgreements {

    public class LeaseAgreementFeeVM {

        public int Id { get; set; }
        public Guid? ReservationId { get; set; }
        public decimal NetAmount { get; set; }
        public decimal VatAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public bool IsCash { get; set; }
        public bool IsSurprise { get; set; }

    }

}