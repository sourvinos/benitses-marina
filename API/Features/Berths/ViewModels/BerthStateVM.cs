namespace API.Features.Reservations.Berths {

    public class BerthAvailableListVM {

        public int Id { get; set; }
        public string Description { get; set; }
        public string BoatName { get; set; }
        public string ToDate { get; set; }
        public bool IsAthenian { get; set; }
        public bool IsOverdue { get; set; }

    }

}