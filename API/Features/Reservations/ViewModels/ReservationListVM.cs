using System;
using System.Collections.Generic;
using API.Infrastructure.Classes;

namespace API.Features.Reservations {

    public class ReservationListVM {

        public Guid ReservationId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public ReservationBoatDetails Boat { get; set; }
        public bool IsDocked { get; set; }
        public bool IsLongTerm { get; set; }
        public bool IsAthenian { get; set; }
        public bool IsOverdue { get; set; }
        public List<ReservationBerthVM> Berths { get; set; }
        public ReservationListOwnerVM Owner { get; set; }
        public SimpleEntity PaymentStatus { get; set; }

    }

}