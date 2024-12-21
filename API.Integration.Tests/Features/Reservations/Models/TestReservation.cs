using System;
using System.Collections.Generic;

namespace Reservations {

    public class TestReservation {

        public int StatusCode { get; set; }

        public Guid ReservationId { get; set; }
        public int PaymentStatusId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Remarks { get; set; }
        public string FinancialRemarks { get; set; }
        public TestReservationBoat Boat { get; set; }
        public TestReservationInsurance Insurance { get; set; }
        public TestReservationOwner Owner { get; set; }
        public TestReservationBilling Billing { get; set; }
        public TestReservationFee Fee { get; set; }
        public List<TestReservationBerth> Berths { get; set; }
        public bool IsDocked { get; set; }
        public bool IsDryDock { get; set; }
        public bool IsAthenian { get; set; }
        public string PutAt { get; set; }

    }

}