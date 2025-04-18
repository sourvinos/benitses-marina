using System;

namespace API.Features.Leases {

    public class LeasePdfFeeVM {

        public int Id { get; set; }
        public Guid? ReservationId { get; set; }
        public decimal NetAmount { get; set; }
        public decimal VatPercent { get; set; }
        public decimal VatAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public bool IsCash { get; set; }

    }

}