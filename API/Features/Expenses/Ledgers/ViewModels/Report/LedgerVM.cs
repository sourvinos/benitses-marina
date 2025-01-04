using API.Infrastructure.Classes;

namespace API.Features.Expenses.Ledgers {

    public class LedgerVM {

        public string Date { get; set; }
        public SimpleEntity Supplier { get; set; }
        public DocumentTypeVM DocumentType { get; set; }
        public PaymentMethodVM PaymentMethod { get; set; }
        public bool HasDocument { get; set; }
        public string DocumentName { get; set; }
        public string InvoiceNo { get; set; }
        public decimal Amount { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }

    }

}