using System;

namespace API.Features.Reservations.Piers {

    public class PierStateVM {

        public int Id { get; set; }
        public string Description { get; set; }
        public string BoatName { get; set; }
        public DateTime? To { get; set; }

    }

}