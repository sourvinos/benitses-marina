using System;
using Infrastructure;

namespace Bookings {

    public class TestBooking : ITestEntity {

        public int StatusCode { get; set; }

        public int Id { get; set; }
        public Guid BookingId { get; set; }
        public int BoatTypeId { get; set; }
        public string BoatName { get; set; }
        public decimal BoatLength { get; set; }
        public decimal BoatWidth { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int StayDuration { get; set; }
        public string ContactDetails { get; set; }
        public string Email { get; set; }
        public string Remarks { get; set; }
        public bool IsConfirmed { get; set; }
        public string PutAt { get; set; }

    }

}