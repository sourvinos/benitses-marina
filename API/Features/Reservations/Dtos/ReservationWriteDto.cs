using System;
using System.Collections.Generic;
using API.Infrastructure.Interfaces;

namespace API.Features.Reservations {

    public class ReservationWriteDto : IMetadata {

        public Guid ReservationId { get; set; }
        public int PaymentStatusId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Remarks { get; set; }
        public string FinancialRemarks { get; set; }
        public bool IsDocked { get; set; }
        public bool IsLongTerm { get; set; }
        public bool IsAthenian { get; set; }
        public ReservationBoatDetailsDto Boat { get; set; }
        public ReservationInsuranceDetailsDto Insurance { get; set; }
        public ReservationOwnerDetailsDto Owner { get; set; }
        public ReservationBillingDetailsDto Billing { get; set; }
        public ReservationFeeDetailsDto Fee { get; set; }
        public List<ReservationBerthWriteDto> Berths { get; set; }
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }

    }

}