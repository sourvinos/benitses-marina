using System;
using System.Collections.Generic;
using API.Features.Reservations.PaymentStatuses;
using API.Infrastructure.Interfaces;

namespace API.Features.Reservations {

    public class Reservation : IMetadata {

        public Guid ReservationId { get; set; }
        public int PaymentStatusId { get; set; }
        public string BoatName { get; set; }
        public string Loa { get; set; }
        public string Beam { get; set; }
        public string Draft { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Remarks { get; set; }
        public string FinancialRemarks { get; set; }
        public bool IsDocked { get; set; }
        public bool IsLongTerm { get; set; }
        public bool IsAthenian { get; set; }
        public ReservationLease ReservationLease { get; set; }
        public ReservationOwner ReservationOwner { get; set; }
        public List<ReservationBerth> Berths { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }

    }

}