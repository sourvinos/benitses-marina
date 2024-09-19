using System;
using System.Collections.Generic;

namespace API.Features.Reservations.Bookings {

    public class BookingListVM {

        public Guid BookingId { get; set; }
        public string BoatName { get; set; }
        public string BoatLength { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsDocked { get; set; }
        public bool IsPaid { get; set; }
        public List<BookingPierVM> Piers { get; set; }

    }

}