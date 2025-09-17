using API.Features.Reservations.PaymentStatuses;
using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.Transactions {

    public class Reservation : IMetadata {

        // PK
        public Guid ReservationId { get; set; }
        // FKs
        public int PaymentStatusId { get; set; }
        // Fields
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Remarks { get; set; }
        public string FinancialRemarks { get; set; }
        public bool IsAthenian { get; set; }
        public bool IsDocked { get; set; }
        public bool IsDryDock { get; set; }
        public bool IsRequest { get; set; }
        // Children
        public ReservationBoat Boat { get; set; }
        public ReservationInsurance Insurance { get; set; }
        public ReservationOwner Owner { get; set; }
        public ReservationBilling Billing { get; set; }
        public ReservationFee Fee { get; set; }
        public List<ReservationBerth> Berths { get; set; }
        // Navigation
        public PaymentStatus PaymentStatus { get; set; }
        // Metadata
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }

    }

}