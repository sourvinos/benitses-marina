using System;

namespace API.Features.Sales.Invoices {

    public class InvoiceItem {

        public Guid InvoiceId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string TaxCode { get; set; }
        public int Quantity { get; set; }
        public decimal NetPrice { get; set; }
        public decimal GrossPrice { get; set; }

    }

}