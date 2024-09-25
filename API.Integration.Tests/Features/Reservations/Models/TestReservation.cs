using System;
using System.Collections.Generic;
using Infrastructure;

namespace Reservations {

    public class TestReservation : ITestEntity {

        public int StatusCode { get; set; }

        public int Id { get; set; }
        public Guid ReservationId { get; set; }
        public int BoatTypeId { get; set; }
        public string BoatName { get; set; }
        public string Loa { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int Days { get; set; }
        public string Email { get; set; }
        public string Remarks { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsDocked { get; set; }
        public bool IsPaid { get; set; }
        public List<TestReservationPier> Piers { get; set; }
        public string PutAt { get; set; }

    }

}