using System;
using System.Collections.Generic;
using API.Infrastructure.Classes;
using API.Infrastructure.Interfaces;

namespace API.Features.Reservations {

    public class ReservationReadDto : IMetadata {

        // PK
        public Guid ReservationId { get; set; }
        // Fields
        public string BoatName { get; set; }
        public string Customer { get; set; }
        public string Loa { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int Days { get; set; }
        public string Email { get; set; }
        public string Remarks { get; set; }
        public string FinancialRemarks { get; set; }
        public string Contact { get; set; }
        public bool IsDocked { get; set; }
        public bool IsLongTerm { get; set; }
        public bool IsAthenian { get; set; }
        public List<ReservationBerthVM> Berths { get; set; }
        // Navigation
        public SimpleEntity PaymentStatus { get; set; }
        // Metadata
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }

    }

}