using System;

namespace API.Features.Reservations.Transactions {

    public class ReservationBerthWriteDto {

        public int Id { get; set; }
        public Guid? ReservationId { get; set; }
        public string Description { get; set; }

    }

}