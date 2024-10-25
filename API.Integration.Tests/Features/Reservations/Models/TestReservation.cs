using System;
using System.Collections.Generic;

namespace Reservations {

    public class TestReservation {

        public int StatusCode { get; set; }

        public Guid ReservationId { get; set; }
        public int PaymentStatusId { get; set; }
        public string BoatName { get; set; }
        public string Loa { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int Days { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
        public string Remarks { get; set; }
        public string FinancialRemarks { get; set; }
        public bool IsDocked { get; set; }
        public bool IsLongTerm { get; set; }
        public bool IsAthenian { get; set; }
        public TestReservationLease ReservationLease { get; set; }
        public List<TestReservationBerth> Berths { get; set; }
        public string PutAt { get; set; }

    }

}