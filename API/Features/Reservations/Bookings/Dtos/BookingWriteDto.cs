using System;
using System.Collections.Generic;
using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.Bookings {

    public class BookingWriteDto : IMetadata {

        // PK
        public Guid BookingId { get; set; }
        // FKs
        public int BoatTypeId { get; set; }
        //  Fields
        public string BoatName { get; set; }
        public decimal BoatLength { get; set; }
        public decimal BoatWidth { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int StayDuration { get; set; }
        public string ContactDetails { get; set; }
        public string Email { get; set; }
        public string Remarks { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsDocked { get; set; }
        public bool IsPaid { get; set; }
        public List<BookingPier> Piers { get; set; }
        // Metadata
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }

    }

}