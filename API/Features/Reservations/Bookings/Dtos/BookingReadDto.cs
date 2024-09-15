using System;
using System.Collections.Generic;
using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.Bookings {

    public class BookingReadDto : IMetadata {

        // PK
        public Guid BookingId { get; set; }
        // FKs
        public int BoatTypeId { get; set; }
        // Fields
        public string BoatName { get; set; }
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int Days { get; set; }
        public string ContactDetails { get; set; }
        public string Email { get; set; }
        public string Remarks { get; set; }
        public bool IsConfirmed { get; set; }
        public List<BookingPierVM> Piers { get; set; }
        // Metadata
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }

    }

}