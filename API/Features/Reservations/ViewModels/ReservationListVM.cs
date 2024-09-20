using System;
using System.Collections.Generic;

namespace API.Features.Reservations {

    public class ReservationListVM {

        public Guid ReservationId { get; set; }
        public string BoatName { get; set; }
        public string BoatLength { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsDocked { get; set; }
        public bool IsPaid { get; set; }
        public List<ReservationPierVM> Piers { get; set; }

    }

}