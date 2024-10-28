using System;

namespace API.Features.Reservations {

    public class ReservationBillingDetails {

        public int Id { get; set; }
        public Guid ReservationId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string TaxNo { get; set; }
        public string TaxOffice { get; set; }
        public string PassportNo { get; set; }
        public string Phones { get; set; }
        public string Email { get; set; }

    }

}