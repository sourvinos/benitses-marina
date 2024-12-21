using System;
using System.Collections.Generic;
using API.Infrastructure.Classes;
using API.Infrastructure.Interfaces;

namespace API.Features.Reservations {

    public class ReservationReadDto : IMetadata {

        public Guid ReservationId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public ReservationBoatDetailsDto Boat { get; set; }
        public ReservationInsuranceDetailsDto Insurance { get; set; }
        public ReservationOwnerDetailsDto Owner { get; set; }
        public ReservationBillingDetailsDto Billing { get; set; }
        public ReservationFeeDetailsDto Fee { get; set; }
        public List<ReservationBerthVM> Berths { get; set; }
        public string Remarks { get; set; }
        public string FinancialRemarks { get; set; }
        public bool IsDocked { get; set; }
        public bool IsDryDock { get; set; }
        public bool IsAthenian { get; set; }
        public SimpleEntity PaymentMethod { get; set; }
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }

    }

}