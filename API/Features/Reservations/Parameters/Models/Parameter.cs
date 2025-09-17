using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.Parameters {

    public class ReservationParameter : IMetadata {

        // PK
        public Guid Id { get; set; }
        // Fields
        public string Phones { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        // Metadata
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }

    }

}