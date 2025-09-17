using System;
using System.Collections.Generic;
using API.Infrastructure.Classes;

namespace API.Features.Reservations.Transactions {

    public class ReservationListVM {

        public Guid ReservationId { get; set; }
        public string BoatName { get; set; }
        public string OwnerName { get; set; }
        public string BoatLoa { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public List<ReservationBerthVM> Berths { get; set; }
        public SimpleEntity PaymentStatus { get; set; }
        public bool IsAthenian { get; set; }
        public bool IsDocked { get; set; }
        public bool IsDryDock { get; set; }
        public bool IsFishingBoat { get; set; }
        public bool IsOverdue { get; set; }
        public bool IsRequest { get; set; }

    }

}