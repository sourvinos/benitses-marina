using API.Infrastructure.Classes;
using API.Infrastructure.Interfaces;

namespace API.Features.Expenses.Transactions {

    public class ExpenseReadDto : IMetadata {

        // PK
        public Guid ExpenseId { get; set; }
        // FKs
        public int CompanyId { get; set; }
        public int DocumentTypeId { get; set; }
        public int PaymentMethodId { get; set; }
        public int SupplierId { get; set; }
        // Navigation
        public SimpleEntity Company { get; set; }
        public SimpleEntity DocumentType { get; set; }
        public SimpleEntity PaymentMethod { get; set; }
        public SimpleEntity Supplier { get; set; }
        // Fields
        public string Date { get; set; }
        public string DocumentNo { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
        public bool IsDeleted { get; set; }
        // Metadata
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }

    }

}