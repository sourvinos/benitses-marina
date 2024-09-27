using System;
using System.Collections.Generic;
using API.Infrastructure.Classes;

namespace API.Features.Reservations {

    public class ReservationListVM {

        public Guid ReservationId { get; set; }
        public string BoatName { get; set; }
        public string Loa { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int Days { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsDocked { get; set; }
        public bool IsPaid { get; set; }
        public SimpleEntity BoatType { get; set; }
        public List<ReservationPierVM> Piers { get; set; }

    }

}