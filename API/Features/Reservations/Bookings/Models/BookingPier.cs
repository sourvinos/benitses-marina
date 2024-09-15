using System;

namespace API.Features.Reservations.Bookings {

    public class BookingPier {

        public int Id { get; set; }
        public Guid BookingId { get; set; }
        public string Description { get; set; }

    }

}