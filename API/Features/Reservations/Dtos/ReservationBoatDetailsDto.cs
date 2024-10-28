using System;

namespace API.Features.Reservations {

    public class ReservationBoatDetailsDto {

        public int Id { get; set; }
        public Guid? ReservationId { get; set; }
        public string Name { get; set; }
        public string Flag { get; set; }
        public string Loa { get; set; }
        public string Beam { get; set; }
        public string Draft { get; set; }
        public string RegistryPort { get; set; }
        public string RegistryNo { get; set; }
        public string Type { get; set; }
        public string Usage { get; set; }

    }

}