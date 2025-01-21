using System;

namespace API.Features.Sales.Transactions {

    public class SaleUpdateAadeDto {

        // FKs
        public Guid InvoiceId { get; set; }
        // Fields
        public string Uid { get; set; }
        public string Mark { get; set; }
        public string MarkCancel { get; set; }
        public string QrUrl { get; set; }

    }

}