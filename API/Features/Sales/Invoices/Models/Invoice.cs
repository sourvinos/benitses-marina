using API.Features.Sales.Transactions;

namespace API.Features.Sales.Invoices {

    public class Invoice : TransactionsBase {

        public List<InvoiceItem> Items { get; set; }

    }

}