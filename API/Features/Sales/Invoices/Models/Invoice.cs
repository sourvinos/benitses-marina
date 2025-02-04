using System.Collections.Generic;
using API.Features.Sales.Transactions;

namespace API.Features.Sales.Invoices {

    public class Invoice : TransactionsBase {

        // public InvoiceAade Aade { get; set; }
        public List<InvoiceItem> Items { get; set; }

    }

}