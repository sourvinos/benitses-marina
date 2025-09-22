using System;
using API.Infrastructure.Classes;

namespace API.Features.Leases {

    public class LeasePdfBoatVM {

        public int Id { get; set; }
        public Guid? ReservationId { get; set; }
        public SimpleEntity Type { get; set; }
        public SimpleEntity Usage { get; set; }
        public bool IsFishingBoat { get; set; }
        public string Name { get; set; }
        public string Flag { get; set; }
        public string Loa { get; set; }
        public string Beam { get; set; }
        public string Draft { get; set; }
        public string RegistryPort { get; set; }
        public string RegistryNo { get; set; }

    }

}