using System;

namespace API.Features.Sales.Invoices {

    public class InvoiceItemWriteDto {

        public int Id { get; set; }
        public Guid InvoiceId { get; set; }
        public decimal GrossAmount { get; set; }

    }

}