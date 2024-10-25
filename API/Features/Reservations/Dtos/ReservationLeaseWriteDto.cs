using System;

namespace API.Features.Reservations {

    public class ReservationLeaseWriteDto {

        public int Id { get; set; }
        public Guid? ReservationId { get; set; }
        public string Customer { get; set; }
        public string InsuranceCompany { get; set; }
        public string PolicyNo { get; set; }
        public string PolicyEnds { get; set; }
        public string Flag { get; set; }
        public string RegistryPort { get; set; }
        public string RegistryNo { get; set; }
        public string BoatType { get; set; }
        public string BoatUsage { get; set; }
        public decimal NetAmount { get; set; }
        public decimal VatAmount { get; set; }
        public decimal GrossAmount { get; set; }
        
    }

}