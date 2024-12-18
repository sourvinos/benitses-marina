using API.Infrastructure.Interfaces;

namespace API.Features.Suppliers {

    public class SupplierWriteDto : IBaseEntity, IMetadata {

        // PK
        public int Id { get; set; }
        // Fields
        public string Description { get; set; }
        public string VatNumber { get; set; }
        public string Phones { get; set; }
        public string Email { get; set; }
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        // Metadata
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }

    }

}