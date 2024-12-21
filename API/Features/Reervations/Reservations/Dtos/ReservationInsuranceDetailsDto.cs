using System;

namespace API.Features.Reservations {

    public class ReservationInsuranceDetailsDto {

        public int Id { get; set; }
        public Guid? ReservationId { get; set; }
        public string InsuranceCompany { get; set; }
        public string PolicyNo { get; set; }
        public string PolicyEnds { get; set; }
        
    }

}