namespace API.Features.Leases {

    public class LeaseEndingListVM {

        public string ReservationId { get; set; }
        public LeaseEndingBoatListVM Boat { get; set; }
        public string LeaseEnds { get; set; }

    }

}