using System;
using API.Features.BoatTypes;
using API.Features.BoatUsages;

namespace API.Features.Reservations {

    public class ReservationBoat {

        public int Id { get; set; }
        public Guid ReservationId { get; set; }
        public int TypeId { get; set; }
        public int UsageId { get; set; }
        public string Name { get; set; }
        public string Flag { get; set; }
        public string Loa { get; set; }
        public string Beam { get; set; }
        public string Draft { get; set; }
        public string RegistryPort { get; set; }
        public string RegistryNo { get; set; }
        public BoatType Type { get; set; }
        public BoatUsage Usage { get; set; }

    }

}