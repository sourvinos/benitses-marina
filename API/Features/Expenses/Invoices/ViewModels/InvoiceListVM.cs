using API.Infrastructure.Classes;

namespace API.Features.Expenses.Invoices {

    public class InvoiceListVM {

        public string Id { get; set; }
        public string Date { get; set; }
        public SimpleEntity Company { get; set; }
        public SimpleEntity DocumentType { get; set; }
        public SimpleEntity PaymentMethod { get; set; }
        public SimpleEntity Supplier { get; set; }
        public string DocumentNo { get; set; }
        public decimal Amount { get; set; }
        public bool HasDocument { get; set; }

    }

}