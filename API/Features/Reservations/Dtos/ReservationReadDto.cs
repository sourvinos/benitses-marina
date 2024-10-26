using System;
using System.Collections.Generic;
using API.Infrastructure.Classes;
using API.Infrastructure.Interfaces;

namespace API.Features.Reservations {

    public class ReservationReadDto : IMetadata {

        public Guid ReservationId { get; set; }
        public string BoatName { get; set; }
        public string Loa { get; set; }
        public string Beam { get; set; }
        public string Draft { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Remarks { get; set; }
        public string FinancialRemarks { get; set; }
        public bool IsDocked { get; set; }
        public bool IsLongTerm { get; set; }
        public bool IsAthenian { get; set; }
        public ReservationOwnerReadDto ReservationOwner { get; set; }
        public ReservationLeaseReadDto ReservationLease { get; set; }
        public List<ReservationBerthVM> Berths { get; set; }
        public SimpleEntity PaymentStatus { get; set; }
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }

    }

}