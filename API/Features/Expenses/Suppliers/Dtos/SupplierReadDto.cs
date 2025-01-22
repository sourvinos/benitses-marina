using API.Infrastructure.Classes;
using API.Infrastructure.Interfaces;

namespace API.Features.Expenses.Suppliers {

    public class SupplierReadDto : IBaseEntity, IMetadata {

        // PK
        public int Id { get; set; }
        // FKs
        public int BankId { get; set; }
        // Navigation
        public SimpleEntity Bank { get; set; }
        // Fields
        public string Description { get; set; }
        public string LongDescription { get; set; }
        public string VatNumber { get; set; }
        public string Phones { get; set; }
        public string Email { get; set; }
        public string Iban { get; set; }
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        // Metadata
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }

    }

}