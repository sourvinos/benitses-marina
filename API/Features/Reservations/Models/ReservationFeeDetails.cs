using System;

namespace API.Features.Reservations {

    public class ReservationFeeDetails {

        public int Id { get; set; }
        public Guid ReservationId { get; set; }
        public decimal NetAmount { get; set; }
        public decimal VatAmount { get; set; }
        public decimal GrossAmount { get; set; }

    }

}