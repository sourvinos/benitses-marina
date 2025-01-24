using System.Collections.Generic;
using API.Features.Sales.Transactions;

namespace API.Features.Sales.Invoices {

    public class InvoiceWriteDto : TransactionsBase {

        public List<InvoiceItemWriteDto> InvoicesPorts { get; set; }

    }

}