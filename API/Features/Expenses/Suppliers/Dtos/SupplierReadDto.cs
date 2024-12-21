using API.Infrastructure.Classes;
using API.Infrastructure.Interfaces;

namespace API.Features.Expenses.Suppliers {

    public class SupplierReadDto : IBaseEntity, IMetadata {

        public int Id { get; set; }
        public int BankId { get; set; }
        public string Description { get; set; }
        public string VatNumber { get; set; }
        public string Phones { get; set; }
        public string Email { get; set; }
        public string Iban { get; set; }
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        public SimpleEntity Bank { get; set; }
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }

    }

}