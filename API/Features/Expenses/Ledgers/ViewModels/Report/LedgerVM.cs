using API.Infrastructure.Classes;

namespace API.Features.Expenses.Ledgers {

    public class LedgerVM {

        public string Date { get; set; }
        public SimpleEntity Supplier { get; set; }
        public SimpleEntity DocumentType { get; set; }
        public string InvoiceNo { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }

    }

}